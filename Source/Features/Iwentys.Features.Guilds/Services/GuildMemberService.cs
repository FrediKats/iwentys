using System;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Features.GithubIntegration.Services;
using Iwentys.Features.Guilds.Domain;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Guilds.Models.Guilds;
using Iwentys.Features.Guilds.Repositories;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Guilds.Services
{
    public class GuildMemberService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericRepository<StudentEntity> _studentRepository;
        private readonly IGenericRepository<TributeEntity> _guildTributeRepository;

        private readonly GithubIntegrationService _githubIntegrationService;
        private readonly IGuildRepository _guildRepository;
        private readonly IGuildMemberRepository _guildMemberRepository;

        public GuildMemberService(GithubIntegrationService githubIntegrationService, IGuildRepository guildRepository, IGuildMemberRepository guildMemberRepository, IUnitOfWork unitOfWork)
        {
            _githubIntegrationService = githubIntegrationService;
            _guildRepository = guildRepository;
            _guildMemberRepository = guildMemberRepository;
            
            _unitOfWork = unitOfWork;
            _studentRepository = _unitOfWork.GetRepository<StudentEntity>();
            _guildTributeRepository = _unitOfWork.GetRepository<TributeEntity>();
        }

        public async Task<GuildProfileDto> EnterGuildAsync(AuthorizedUser user, int guildId)
        {
            GuildDomain guild = CreateDomain(await _guildRepository.GetAsync(guildId));

            if (await guild.GetUserMembershipState(user.Id) != UserMembershipState.CanEnter)
                throw new InnerLogicException($"Student unable to enter this guild! UserId: {user.Id} GuildId: {guildId}");

            StudentEntity profile = await _studentRepository.GetByIdAsync(user.Id);
            await _guildMemberRepository.AddMemberAsync(guild.Profile, profile, GuildMemberType.Member);

            return await Get(guildId, user.Id);
        }

        public async Task<GuildProfileDto> RequestGuildAsync(AuthorizedUser user, int guildId)
        {
            GuildDomain guild = CreateDomain(await _guildRepository.GetAsync(guildId));

            if (await guild.GetUserMembershipState(user.Id) != UserMembershipState.CanRequest)
                throw new InnerLogicException($"Student unable to send request to this guild! UserId: {user.Id} GuildId: {guildId}");

            StudentEntity profile = await _studentRepository.GetByIdAsync(user.Id);
            await _guildMemberRepository.AddMemberAsync(guild.Profile, profile, GuildMemberType.Requested);
            return await Get(guildId, user.Id);
        }

        public async Task LeaveGuildAsync(AuthorizedUser user, int guildId)
        {
            GuildEntity studentGuild = _guildRepository.ReadForStudent(user.Id);
            if (studentGuild is null || studentGuild.Id != guildId)
                throw InnerLogicException.Guild.IsNotGuildMember(user.Id, guildId);

            TributeEntity userTribute = _guildTributeRepository.GetAsync()
                .Where(t => t.GuildId == guildId)
                .Where(t => t.ProjectEntity.StudentId == user.Id)
                .SingleOrDefault(t => t.State == TributeState.Active);
            
            if (userTribute is not null)
                await _guildTributeRepository.DeleteAsync(userTribute.ProjectId);

            _guildMemberRepository.RemoveMemberAsync(guildId, user.Id);
        }

        public async Task<GuildMemberEntity[]> GetGuildRequests(AuthorizedUser user, int guildId)
        {
            StudentEntity student = await _studentRepository.GetByIdAsync(user.Id);
            GuildEntity guild = await _guildRepository.GetAsync(guildId);
            student.EnsureIsGuildEditor(guild);

            return guild.Members
                .Where(m => m.MemberType == GuildMemberType.Requested)
                .ToArray();
        }

        public async Task<GuildMemberEntity[]> GetGuildBlocked(AuthorizedUser user, int guildId)
        {
            StudentEntity student = await _studentRepository.GetByIdAsync(user.Id);
            GuildEntity guild = await _guildRepository.GetAsync(guildId);
            student.EnsureIsGuildEditor(guild);

            return guild.Members
                .Where(m => m.MemberType == GuildMemberType.Blocked)
                .ToArray();
        }

        public async Task BlockGuildMember(AuthorizedUser user, int guildId, int memberId)
        {
            GuildDomain guildDomain = CreateDomain(await _guildRepository.GetAsync(guildId));
            GuildMemberEntity memberToKick = await guildDomain.EnsureMemberCanRestrictPermissionForOther(user, memberId);
            memberToKick.MarkBlocked();
            await _guildMemberRepository.UpdateMemberAsync(memberToKick);
        }

        public async Task UnblockStudent(AuthorizedUser user, int guildId, int studentId)
        {
            StudentEntity student = await _studentRepository.GetByIdAsync(user.Id);
            GuildEntity guild = await _guildRepository.GetAsync(guildId);
            student.EnsureIsGuildEditor(guild);

            GuildMemberEntity member = guild.Members.Find(m => m.MemberId == studentId);

            if (member is null || member.MemberType != GuildMemberType.Blocked)
                throw new InnerLogicException($"Student is not blocked in guild! StudentId: {studentId} GuildId: {guildId}");

            _guildMemberRepository.RemoveMemberAsync(guildId, studentId);
        }

        public async Task KickGuildMemberAsync(AuthorizedUser user, int guildId, int memberId)
        {
            GuildDomain guildDomain = CreateDomain(await _guildRepository.GetAsync(guildId));
            GuildMemberEntity memberToKick = await guildDomain.EnsureMemberCanRestrictPermissionForOther(user, memberId);

            memberToKick.Member.GuildLeftTime = DateTime.UtcNow.ToUniversalTime();
            _guildMemberRepository.RemoveMemberAsync(guildId, memberId);
        }

        public async Task AcceptRequest(AuthorizedUser user, int guildId, int memberForAccepting)
        {
            StudentEntity student = await _studentRepository.GetByIdAsync(user.Id);
            GuildEntity guild = await _guildRepository.GetAsync(guildId);
            student.EnsureIsGuildEditor(guild);

            GuildMemberEntity member = guild.Members.Find(m => m.MemberId == memberForAccepting);

            if (member is null || member.MemberType != GuildMemberType.Requested)
                throw InnerLogicException.Guild.RequestWasNotFound(memberForAccepting, guildId);

            member.MemberType = GuildMemberType.Member;

            await _guildMemberRepository.UpdateMemberAsync(member);
        }

        public async Task RejectRequest(AuthorizedUser user, int guildId, int studentId)
        {
            StudentEntity initiator = await _studentRepository.GetByIdAsync(user.Id);
            GuildEntity guild = await _guildRepository.GetAsync(guildId);
            initiator.EnsureIsGuildEditor(guild);

            GuildMemberEntity member = guild.Members.Find(m => m.MemberId == studentId);

            if (member is null || member.MemberType != GuildMemberType.Requested)
                throw InnerLogicException.Guild.RequestWasNotFound(studentId, guildId);

            _guildMemberRepository.RemoveMemberAsync(guildId, studentId);
        }

        public async Task<ExtendedGuildProfileWithMemberDataDto> Get(int id, int? userId)
        {
            GuildEntity guild = await _guildRepository.GetAsync(id);
            return await CreateDomain(guild).ToExtendedGuildProfileDto(userId);
        }

        private GuildDomain CreateDomain(GuildEntity guild)
        {
            return new GuildDomain(guild, _githubIntegrationService, _studentRepository, _guildRepository, _guildMemberRepository);
        }
    }
}
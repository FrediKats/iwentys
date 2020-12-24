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
        private readonly IGenericRepository<GuildEntity> _guildRepositoryNew;
        private readonly IGenericRepository<GuildMemberEntity> _guildMemberRepository;

        private readonly GithubIntegrationService _githubIntegrationService;

        public GuildMemberService(GithubIntegrationService githubIntegrationService, IUnitOfWork unitOfWork)
        {
            _githubIntegrationService = githubIntegrationService;

            _unitOfWork = unitOfWork;
            _studentRepository = _unitOfWork.GetRepository<StudentEntity>();
            _guildRepositoryNew = _unitOfWork.GetRepository<GuildEntity>();
            _guildTributeRepository = _unitOfWork.GetRepository<TributeEntity>();
            _guildMemberRepository = _unitOfWork.GetRepository<GuildMemberEntity>();
        }

        public async Task<GuildProfileDto> EnterGuildAsync(AuthorizedUser user, int guildId)
        {
            GuildDomain guild = CreateDomain(await _guildRepositoryNew.GetByIdAsync(guildId));

            if (await guild.GetUserMembershipState(user.Id) != UserMembershipState.CanEnter)
                throw new InnerLogicException($"Student unable to enter this guild! UserId: {user.Id} GuildId: {guildId}");

            StudentEntity profile = await _studentRepository.GetByIdAsync(user.Id);
            var guildMemberEntity = new GuildMemberEntity(guild.Profile, profile, GuildMemberType.Member);
            await _guildMemberRepository.InsertAsync(guildMemberEntity);
            await _unitOfWork.CommitAsync();

            return await Get(guildId, user.Id);
        }

        public async Task<GuildProfileDto> RequestGuildAsync(AuthorizedUser user, int guildId)
        {
            GuildDomain guild = CreateDomain(await _guildRepositoryNew.GetByIdAsync(guildId));

            if (await guild.GetUserMembershipState(user.Id) != UserMembershipState.CanRequest)
                throw new InnerLogicException($"Student unable to send request to this guild! UserId: {user.Id} GuildId: {guildId}");

            StudentEntity profile = await _studentRepository.GetByIdAsync(user.Id);
            var guildMemberEntity = new GuildMemberEntity(guild.Profile, profile, GuildMemberType.Requested);
            await _guildMemberRepository.InsertAsync(guildMemberEntity);
            await _unitOfWork.CommitAsync();

            return await Get(guildId, user.Id);
        }

        public async Task LeaveGuildAsync(AuthorizedUser user, int guildId)
        {
            GuildEntity studentGuild = _guildMemberRepository.ReadForStudent(user.Id);
            if (studentGuild is null || studentGuild.Id != guildId)
                throw InnerLogicException.Guild.IsNotGuildMember(user.Id, guildId);

            TributeEntity userTribute = _guildTributeRepository.GetAsync()
                .Where(t => t.GuildId == guildId)
                .Where(t => t.ProjectEntity.StudentId == user.Id)
                .SingleOrDefault(t => t.State == TributeState.Active);
            
            if (userTribute is not null)
                await _guildTributeRepository.DeleteAsync(userTribute.ProjectId);

            await RemoveMemberAsync(guildId, user.Id);
        }

        public async Task<GuildMemberEntity[]> GetGuildRequests(AuthorizedUser user, int guildId)
        {
            StudentEntity student = await _studentRepository.GetByIdAsync(user.Id);
            GuildEntity guild = await _guildRepositoryNew.GetByIdAsync(guildId);
            student.EnsureIsGuildEditor(guild);

            return guild.Members
                .Where(m => m.MemberType == GuildMemberType.Requested)
                .ToArray();
        }

        public async Task<GuildMemberEntity[]> GetGuildBlocked(AuthorizedUser user, int guildId)
        {
            StudentEntity student = await _studentRepository.GetByIdAsync(user.Id);
            GuildEntity guild = await _guildRepositoryNew.GetByIdAsync(guildId);
            student.EnsureIsGuildEditor(guild);

            return guild.Members
                .Where(m => m.MemberType == GuildMemberType.Blocked)
                .ToArray();
        }

        public async Task BlockGuildMember(AuthorizedUser user, int guildId, int memberId)
        {
            GuildDomain guildDomain = CreateDomain(await _guildRepositoryNew.GetByIdAsync(guildId));
            GuildMemberEntity memberToKick = await guildDomain.EnsureMemberCanRestrictPermissionForOther(user, memberId);
            memberToKick.MarkBlocked();
            _guildMemberRepository.Update(memberToKick);
            await _unitOfWork.CommitAsync();
        }

        public async Task UnblockStudent(AuthorizedUser user, int guildId, int studentId)
        {
            StudentEntity student = await _studentRepository.GetByIdAsync(user.Id);
            GuildEntity guild = await _guildRepositoryNew.GetByIdAsync(guildId);
            student.EnsureIsGuildEditor(guild);

            GuildMemberEntity member = guild.Members.Find(m => m.MemberId == studentId);

            if (member is null || member.MemberType != GuildMemberType.Blocked)
                throw new InnerLogicException($"Student is not blocked in guild! StudentId: {studentId} GuildId: {guildId}");

            await RemoveMemberAsync(guildId, studentId);
        }

        public async Task KickGuildMemberAsync(AuthorizedUser user, int guildId, int memberId)
        {
            GuildDomain guildDomain = CreateDomain(await _guildRepositoryNew.GetByIdAsync(guildId));
            GuildMemberEntity memberToKick = await guildDomain.EnsureMemberCanRestrictPermissionForOther(user, memberId);

            memberToKick.Member.GuildLeftTime = DateTime.UtcNow;
            await RemoveMemberAsync(guildId, memberId);
        }

        public async Task AcceptRequest(AuthorizedUser user, int guildId, int memberForAccepting)
        {
            StudentEntity student = await _studentRepository.GetByIdAsync(user.Id);
            GuildEntity guild = await _guildRepositoryNew.GetByIdAsync(guildId);
            student.EnsureIsGuildEditor(guild);

            GuildMemberEntity member = guild.Members.Find(m => m.MemberId == memberForAccepting);

            if (member is null || member.MemberType != GuildMemberType.Requested)
                throw InnerLogicException.Guild.RequestWasNotFound(memberForAccepting, guildId);

            member.MemberType = GuildMemberType.Member;

            _guildMemberRepository.Update(member);
            await _unitOfWork.CommitAsync();
        }

        public async Task RejectRequest(AuthorizedUser user, int guildId, int studentId)
        {
            StudentEntity initiator = await _studentRepository.GetByIdAsync(user.Id);
            GuildEntity guild = await _guildRepositoryNew.GetByIdAsync(guildId);
            initiator.EnsureIsGuildEditor(guild);

            GuildMemberEntity member = guild.Members.Find(m => m.MemberId == studentId);

            if (member is null || member.MemberType != GuildMemberType.Requested)
                throw InnerLogicException.Guild.RequestWasNotFound(studentId, guildId);

            await RemoveMemberAsync(guildId, studentId);
        }

        public async Task<ExtendedGuildProfileWithMemberDataDto> Get(int id, int? userId)
        {
            GuildEntity guild = await _guildRepositoryNew.GetByIdAsync(id);
            return await CreateDomain(guild).ToExtendedGuildProfileDto(userId);
        }

        public async Task PromoteToEditor(AuthorizedUser creator, int userForPromotion)
        {
            StudentEntity studentCreator = await _studentRepository.GetByIdAsync(creator.Id);

            var guildMemberEntity = _guildMemberRepository.GetStudentMembership(creator.Id);
            studentCreator.EnsureIsGuildEditor(guildMemberEntity);
            
            //TODO: check member state
            var studentMembership = _guildMemberRepository.GetStudentMembership(userForPromotion);
            studentMembership.MemberType = GuildMemberType.Mentor;
            _guildMemberRepository.Update(studentMembership);
            await _unitOfWork.CommitAsync();
        }

        private GuildDomain CreateDomain(GuildEntity guild)
        {
            return new GuildDomain(guild, _githubIntegrationService, _studentRepository, _guildMemberRepository);
        }

        private async Task RemoveMemberAsync(int guildId, int userId)
        {
            GuildMemberEntity guildMember = _guildMemberRepository.GetAsync().Single(gm => gm.GuildId == guildId && gm.MemberId == userId);
            if (guildMember.MemberType == GuildMemberType.Creator)
                throw InnerLogicException.Guild.CreatorCannotLeave(userId, guildId);

            _guildMemberRepository.Delete(guildMember);
            await _unitOfWork.CommitAsync();
        }
    }
}
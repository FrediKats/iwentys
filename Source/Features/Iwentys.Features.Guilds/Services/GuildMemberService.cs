using System;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Iwentys.Common.Tools;
using Iwentys.Features.GithubIntegration.Services;
using Iwentys.Features.Guilds.Domain;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Guilds.Models.Guilds;
using Iwentys.Features.Guilds.Repositories;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Repositories;

namespace Iwentys.Features.Guilds.Services
{
    public class GuildMemberService
    {
        private readonly GithubIntegrationService _githubIntegrationService;
        private readonly IStudentRepository _studentRepository;
        private readonly IGuildRepository _guildRepository;
        private readonly IGuildMemberRepository _guildMemberRepository;
        private readonly IGuildTributeRepository _guildTributeRepository;

        public GuildMemberService(GithubIntegrationService githubIntegrationService, IStudentRepository studentRepository, IGuildRepository guildRepository, IGuildMemberRepository guildMemberRepository, IGuildTributeRepository guildTributeRepository)
        {
            _githubIntegrationService = githubIntegrationService;
            _studentRepository = studentRepository;
            _guildRepository = guildRepository;
            _guildMemberRepository = guildMemberRepository;
            _guildTributeRepository = guildTributeRepository;
        }

        public async Task<GuildProfileDto> EnterGuildAsync(AuthorizedUser user, int guildId)
        {
            GuildDomain guild = CreateDomain(await _guildRepository.GetAsync(guildId));

            if (await guild.GetUserMembershipState(user.Id) != UserMembershipState.CanEnter)
                throw new InnerLogicException($"Student unable to enter this guild! UserId: {user.Id} GuildId: {guildId}");

            StudentEntity profile = await user.GetProfile(_studentRepository);
            await _guildMemberRepository.AddMemberAsync(guild.Profile, profile, GuildMemberType.Member);

            return await Get(guildId, user.Id);
        }

        public async Task<GuildProfileDto> RequestGuildAsync(AuthorizedUser user, int guildId)
        {
            GuildDomain guild = CreateDomain(await _guildRepository.GetAsync(guildId));

            if (await guild.GetUserMembershipState(user.Id) != UserMembershipState.CanRequest)
                throw new InnerLogicException($"Student unable to send request to this guild! UserId: {user.Id} GuildId: {guildId}");

            StudentEntity profile = await user.GetProfile(_studentRepository);
            await _guildMemberRepository.AddMemberAsync(guild.Profile, profile, GuildMemberType.Requested);
            return await Get(guildId, user.Id);
        }

        public async Task LeaveGuildAsync(AuthorizedUser user, int guildId)
        {
            GuildEntity studentGuild = _guildRepository.ReadForStudent(user.Id);
            if (studentGuild == null || studentGuild.Id != guildId)
                throw InnerLogicException.Guild.IsNotGuildMember(user.Id, guildId);

            TributeEntity userTribute = _guildTributeRepository.ReadStudentActiveTribute(studentGuild.Id, user.Id);
            if (userTribute != null)
                await _guildTributeRepository.DeleteAsync(userTribute.ProjectId);

            _guildMemberRepository.RemoveMemberAsync(guildId, user.Id);
        }

        public async Task<GuildMemberEntity[]> GetGuildRequests(AuthorizedUser user, int guildId)
        {
            StudentEntity student = await user.GetProfile(_studentRepository);
            GuildEntity guild = await _guildRepository.GetAsync(guildId);
            student.EnsureIsGuildEditor(guild);

            return guild.Members
                .Where(m => m.MemberType == GuildMemberType.Requested)
                .ToArray();
        }

        public async Task<GuildMemberEntity[]> GetGuildBlocked(AuthorizedUser user, int guildId)
        {
            StudentEntity student = await user.GetProfile(_studentRepository);
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
            StudentEntity student = await user.GetProfile(_studentRepository);
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

        public async Task AcceptRequest(AuthorizedUser user, int guildId, int studentId)
        {
            StudentEntity student = await user.GetProfile(_studentRepository);
            GuildEntity guild = await _guildRepository.GetAsync(guildId);
            student.EnsureIsGuildEditor(guild);

            GuildMemberEntity member = guild.Members.Find(m => m.MemberId == studentId);

            if (member is null || member.MemberType != GuildMemberType.Requested)
                throw InnerLogicException.Guild.RequestWasNotFound(studentId, guildId);

            member.MemberType = GuildMemberType.Member;

            await _guildMemberRepository.UpdateMemberAsync(member);
        }

        public async Task RejectRequest(AuthorizedUser user, int guildId, int studentId)
        {
            StudentEntity initiator = await user.GetProfile(_studentRepository);
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
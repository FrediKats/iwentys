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
using Iwentys.Features.StudentFeature.Domain;
using Iwentys.Features.StudentFeature.Entities;
using Iwentys.Features.StudentFeature.Repositories;

namespace Iwentys.Features.Guilds.Services
{
    public class GuildMemberService
    {
        private readonly GithubUserDataService _githubUserDataService;
        private readonly IStudentRepository _studentRepository;
        private readonly IGuildRepository _guildRepository;
        private readonly IGuildMemberRepository _guildMemberRepository;
        private readonly IGuildTributeRepository _guildTributeRepository;

        public GuildMemberService(GithubUserDataService githubUserDataService, IStudentRepository studentRepository, IGuildRepository guildRepository, IGuildMemberRepository guildMemberRepository, IGuildTributeRepository guildTributeRepository)
        {
            _githubUserDataService = githubUserDataService;
            _studentRepository = studentRepository;
            _guildRepository = guildRepository;
            _guildMemberRepository = guildMemberRepository;
            _guildTributeRepository = guildTributeRepository;
        }

        public async Task<GuildProfileDto> EnterGuildAsync(AuthorizedUser user, Int32 guildId)
        {
            GuildEntity guildEntity = await _guildRepository.GetAsync(guildId);
            GuildDomain guild = CreateDomain(guildEntity);

            if (await guild.GetUserMembershipState(user.Id) != UserMembershipState.CanEnter)
                throw new InnerLogicException($"Student unable to enter this guild! UserId: {user.Id} GuildId: {guildId}");

            StudentEntity profile = await user.GetProfile(_studentRepository);
            await _guildMemberRepository.AddMemberAsync(guild.Profile, profile, GuildMemberType.Member);

            return await Get(guildId, user.Id);
        }

        public async Task<GuildProfileDto> RequestGuildAsync(AuthorizedUser user, Int32 guildId)
        {
            GuildEntity guildEntity = await _guildRepository.GetAsync(guildId);
            GuildDomain guild = CreateDomain(guildEntity);

            if (await guild.GetUserMembershipState(user.Id) != UserMembershipState.CanRequest)
                throw new InnerLogicException($"Student unable to send request to this guild! UserId: {user.Id} GuildId: {guildId}");

            StudentEntity profile = await user.GetProfile(_studentRepository);
            await _guildMemberRepository.AddMemberAsync(guild.Profile, profile, GuildMemberType.Requested);
            return await Get(guildId, user.Id);
        }

        public Task<GuildProfileDto> LeaveGuildAsync(AuthorizedUser user, int guildId)
        {
            GuildEntity studentGuild = _guildRepository.ReadForStudent(user.Id);
            if (studentGuild == null || studentGuild.Id != guildId)
                throw InnerLogicException.Guild.IsNotGuildMember(user.Id, guildId);

            TributeEntity userTribute = _guildTributeRepository.ReadStudentActiveTribute(studentGuild.Id, user.Id);
            if (userTribute != null)
                _guildTributeRepository.DeleteAsync(userTribute.ProjectId);

            _guildMemberRepository.RemoveMemberAsync(guildId, user.Id);

            return Get(guildId, user.Id);
        }

        public async Task<GuildMemberEntity[]> GetGuildRequests(AuthorizedUser user, Int32 guildId)
        {
            StudentEntity student = await user.GetProfile(_studentRepository);
            GuildEntity guild = await _guildRepository.GetAsync(guildId);
            student.EnsureIsGuildEditor(guild);

            return guild.Members
                .Where(m => m.MemberType == GuildMemberType.Requested)
                .ToArray();
        }

        public async Task<GuildMemberEntity[]> GetGuildBlocked(AuthorizedUser user, Int32 guildId)
        {
            StudentEntity student = await user.GetProfile(_studentRepository);
            GuildEntity guild = await _guildRepository.GetAsync(guildId);
            student.EnsureIsGuildEditor(guild);

            return guild.Members
                .Where(m => m.MemberType == GuildMemberType.Blocked)
                .ToArray();
        }

        public async Task BlockGuildMember(AuthorizedUser user, Int32 guildId, Int32 memberId)
        {
            var guildDomain = CreateDomain(await _guildRepository.GetAsync(guildId));
            GuildMemberEntity memberToKick = await guildDomain.EnsureMemberCanRestrictPermissionForOther(user, memberId);

            memberToKick.Member.GuildLeftTime = DateTime.UtcNow.ToUniversalTime();
            memberToKick.MemberType = GuildMemberType.Blocked;
            await _guildMemberRepository.UpdateMemberAsync(memberToKick);
        }

        public async Task UnblockStudent(AuthorizedUser user, Int32 guildId, Int32 studentId)
        {
            StudentEntity student = await user.GetProfile(_studentRepository);
            GuildEntity guild = await _guildRepository.GetAsync(guildId);
            student.EnsureIsGuildEditor(guild);

            GuildMemberEntity member = guild.Members.Find(m => m.MemberId == studentId);

            if (member is null || member.MemberType != GuildMemberType.Blocked)
                throw new InnerLogicException($"Student is not blocked in guild! StudentId: {studentId} GuildId: {guildId}");

            _guildMemberRepository.RemoveMemberAsync(guildId, studentId);
        }

        public async Task KickGuildMemberAsync(AuthorizedUser user, Int32 guildId, Int32 memberId)
        {
            GuildDomain guildDomain = CreateDomain(await _guildRepository.GetAsync(guildId));
            GuildMemberEntity memberToKick = await guildDomain.EnsureMemberCanRestrictPermissionForOther(user, memberId);

            memberToKick.Member.GuildLeftTime = DateTime.UtcNow.ToUniversalTime();
            _guildMemberRepository.RemoveMemberAsync(guildId, memberId);
        }

        public async Task AcceptRequest(AuthorizedUser user, Int32 guildId, Int32 studentId)
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

        public async Task RejectRequest(AuthorizedUser user, Int32 guildId, Int32 studentId)
        {
            StudentEntity student = await user.GetProfile(_studentRepository);
            GuildEntity guild = await _guildRepository.GetAsync(guildId);
            student.EnsureIsGuildEditor(guild);

            GuildMemberEntity member = guild.Members.Find(m => m.MemberId == studentId);

            if (member is null || member.MemberType != GuildMemberType.Requested)
                throw InnerLogicException.Guild.RequestWasNotFound(studentId, guildId);

            _guildMemberRepository.RemoveMemberAsync(guildId, studentId);
        }

        public async Task<GuildProfileDto> Get(int id, int? userId)
        {
            GuildEntity guild = await _guildRepository.GetAsync(id);
            return await CreateDomain(guild).ToGuildProfileDto(userId);
        }

        private GuildDomain CreateDomain(GuildEntity guild)
        {
            return new GuildDomain(guild, _githubUserDataService, _studentRepository, _guildRepository, _guildMemberRepository, _guildTributeRepository);
        }
    }
}
using System;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Core.DomainModel;
using Iwentys.Core.DomainModel.Guilds;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories;
using Iwentys.Integrations.GithubIntegration;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Types;

namespace Iwentys.Core.Services
{
    public class GuildMemberService
    {
        private readonly DatabaseAccessor _database;
        private readonly GithubUserDataService _githubUserDataService;
        private readonly IGithubApiAccessor _githubApiAccessor;

        public GuildMemberService(DatabaseAccessor database, GithubUserDataService githubUserDataService, IGithubApiAccessor githubApiAccessor)
        {
            _database = database;
            _githubUserDataService = githubUserDataService;
            _githubApiAccessor = githubApiAccessor;
        }

        public async Task<GuildProfileDto> EnterGuild(AuthorizedUser user, Int32 guildId)
        {
            GuildEntity guildEntity = await _database.Guild.GetAsync(guildId);
            GuildDomain guild = new GuildDomain(guildEntity, _database, _githubUserDataService, _githubApiAccessor);

            if (await guild.GetUserMembershipState(user.Id) != UserMembershipState.CanEnter)
                throw new InnerLogicException($"Student unable to enter this guild! UserId: {user.Id} GuildId: {guildId}");

            StudentEntity profile = await user.GetProfile(_database.Student);
            _database.GuildMember.AddMember(guild.Profile, profile, GuildMemberType.Member);

            return await Get(guildId, user.Id);
        }

        public async Task<GuildProfileDto> RequestGuild(AuthorizedUser user, Int32 guildId)
        {
            GuildEntity guildEntity = await _database.Guild.GetAsync(guildId);
            GuildDomain guild = new GuildDomain(guildEntity, _database, _githubUserDataService, _githubApiAccessor);

            if (await guild.GetUserMembershipState(user.Id) != UserMembershipState.CanRequest)
                throw new InnerLogicException($"Student unable to send request to this guild! UserId: {user.Id} GuildId: {guildId}");

            StudentEntity profile = await user.GetProfile(_database.Student);
            _database.GuildMember.AddMember(guild.Profile, profile, GuildMemberType.Requested);
            return await Get(guildId, user.Id);
        }

        public Task<GuildProfileDto> LeaveGuild(AuthorizedUser user, int guildId)
        {
            GuildEntity studentGuild = _database.Guild.ReadForStudent(user.Id);
            if (studentGuild == null || studentGuild.Id != guildId)
                throw InnerLogicException.Guild.IsNotGuildMember(user.Id, guildId);

            TributeEntity userTribute = _database.Tribute.ReadStudentActiveTribute(studentGuild.Id, user.Id);
            if (userTribute != null)
                _database.Tribute.DeleteAsync(userTribute.ProjectId);

            _database.GuildMember.RemoveMember(guildId, user.Id);

            return Get(guildId, user.Id);
        }

        public async Task<GuildMemberEntity[]> GetGuildRequests(AuthorizedUser user, Int32 guildId)
        {
            StudentEntity student = await user.GetProfile(_database.Student);
            GuildEntity guild = await _database.Guild.GetAsync(guildId);
            student.EnsureIsGuildEditor(guild);

            return guild.Members
                .Where(m => m.MemberType == GuildMemberType.Requested)
                .ToArray();
        }

        public async Task<GuildMemberEntity[]> GetGuildBlocked(AuthorizedUser user, Int32 guildId)
        {
            StudentEntity student = await user.GetProfile(_database.Student);
            GuildEntity guild = await _database.Guild.GetAsync(guildId);
            student.EnsureIsGuildEditor(guild);

            return guild.Members
                .Where(m => m.MemberType == GuildMemberType.Blocked)
                .ToArray();
        }

        public async Task BlockGuildMember(AuthorizedUser user, Int32 guildId, Int32 memberId)
        {
            var guildDomain = new GuildDomain(await _database.Guild.GetAsync(guildId), _database, _githubUserDataService, _githubApiAccessor);
            GuildMemberEntity memberToKick = await guildDomain.EnsureMemberCanRestrictPermissionForOther(user, memberId);

            memberToKick.Member.GuildLeftTime = DateTime.UtcNow.ToUniversalTime();
            memberToKick.MemberType = GuildMemberType.Blocked;
            _database.GuildMember.UpdateMember(memberToKick);
        }

        public async Task UnblockStudent(AuthorizedUser user, Int32 guildId, Int32 studentId)
        {
            StudentEntity student = await user.GetProfile(_database.Student);
            GuildEntity guild = await _database.Guild.GetAsync(guildId);
            student.EnsureIsGuildEditor(guild);

            GuildMemberEntity member = guild.Members.Find(m => m.MemberId == studentId);

            if (member is null || member.MemberType != GuildMemberType.Blocked)
                throw new InnerLogicException($"Student is not blocked in guild! StudentId: {studentId} GuildId: {guildId}");

            _database.GuildMember.RemoveMember(guildId, studentId);
        }

        public async Task KickGuildMember(AuthorizedUser user, Int32 guildId, Int32 memberId)
        {
            var guildDomain = new GuildDomain(await _database.Guild.GetAsync(guildId), _database, _githubUserDataService, _githubApiAccessor);
            GuildMemberEntity memberToKick = await guildDomain.EnsureMemberCanRestrictPermissionForOther(user, memberId);

            memberToKick.Member.GuildLeftTime = DateTime.UtcNow.ToUniversalTime();
            _database.GuildMember.RemoveMember(guildId, memberId);
        }

        public async Task AcceptRequest(AuthorizedUser user, Int32 guildId, Int32 studentId)
        {
            StudentEntity student = await user.GetProfile(_database.Student);
            GuildEntity guild = await _database.Guild.GetAsync(guildId);
            student.EnsureIsGuildEditor(guild);

            GuildMemberEntity member = guild.Members.Find(m => m.MemberId == studentId);

            if (member is null || member.MemberType != GuildMemberType.Requested)
                throw InnerLogicException.Guild.RequestWasNotFound(studentId, guildId);

            member.MemberType = GuildMemberType.Member;

            _database.GuildMember.UpdateMember(member);
        }

        public async Task RejectRequest(AuthorizedUser user, Int32 guildId, Int32 studentId)
        {
            StudentEntity student = await user.GetProfile(_database.Student);
            GuildEntity guild = await _database.Guild.GetAsync(guildId);
            student.EnsureIsGuildEditor(guild);

            GuildMemberEntity member = guild.Members.Find(m => m.MemberId == studentId);

            if (member is null || member.MemberType != GuildMemberType.Requested)
                throw InnerLogicException.Guild.RequestWasNotFound(studentId, guildId);

            _database.GuildMember.RemoveMember(guildId, studentId);
        }

        public async Task<GuildProfileDto> Get(int id, int? userId)
        {
            GuildEntity guild = await _database.Guild.GetAsync(id);
            return await new GuildDomain(guild, _database, _githubUserDataService, _githubApiAccessor)
                .ToGuildProfileDto(userId);
        }
    }
}
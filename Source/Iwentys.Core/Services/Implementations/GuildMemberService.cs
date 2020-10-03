using System;
using System.Linq;
using Iwentys.Core.DomainModel;
using Iwentys.Core.DomainModel.Guilds;
using Iwentys.Core.GithubIntegration;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Types;

namespace Iwentys.Core.Services.Implementations
{
    public class GuildMemberService : IGuildMemberService
    {
        private readonly DatabaseAccessor _database;
        private readonly IGithubUserDataService _githubUserDataService;
        private readonly IGithubApiAccessor _githubApiAccessor;

        public GuildMemberService(DatabaseAccessor database, IGithubUserDataService githubUserDataService, IGithubApiAccessor githubApiAccessor)
        {
            _database = database;
            _githubUserDataService = githubUserDataService;
            _githubApiAccessor = githubApiAccessor;
        }

        public GuildProfileDto EnterGuild(AuthorizedUser user, Int32 guildId)
        {
            GuildDomain guild = _database.Guild.Get(guildId).To(g => new GuildDomain(g, _database, _githubUserDataService, _githubApiAccessor));

            if (guild.GetUserMembershipState(user.Id) != UserMembershipState.CanEnter)
                throw new InnerLogicException($"Student unable to enter this guild! UserId: {user.Id} GuildId: {guildId}");

            _database.GuildMember.AddMember(guild.Profile, user.GetProfile(_database.Student), GuildMemberType.Member);

            return Get(guildId, user.Id);
        }

        public GuildProfileDto RequestGuild(AuthorizedUser user, Int32 guildId)
        {
            GuildDomain guild = _database.Guild.Get(guildId).To(g =>
                new GuildDomain(g, _database, _githubUserDataService, _githubApiAccessor));

            if (guild.GetUserMembershipState(user.Id) != UserMembershipState.CanRequest)
                throw new InnerLogicException($"Student unable to send request to this guild! UserId: {user.Id} GuildId: {guildId}");

            _database.GuildMember.AddMember(guild.Profile, user.GetProfile(_database.Student), GuildMemberType.Requested);
            return Get(guildId, user.Id);
        }

        public GuildProfileDto LeaveGuild(AuthorizedUser user, int guildId)
        {
            GuildEntity studentGuild = _database.Guild.ReadForStudent(user.Id);
            if (studentGuild == null || studentGuild.Id != guildId)
                throw InnerLogicException.Guild.IsNotGuildMember(user.Id, guildId);

            TributeEntity userTribute = _database.Tribute.ReadStudentActiveTribute(studentGuild.Id, user.Id);
            if (userTribute != null)
                _database.Tribute.Delete(userTribute.ProjectId);

            _database.GuildMember.RemoveMember(guildId, user.Id);

            return Get(guildId, user.Id);
        }

        public GuildMemberEntity[] GetGuildRequests(AuthorizedUser user, Int32 guildId)
        {
            StudentEntity student = user.GetProfile(_database.Student);
            GuildEntity guild = _database.Guild.Get(guildId);
            student.EnsureIsGuildEditor(guild);

            return guild.Members
                .Where(m => m.MemberType == GuildMemberType.Requested)
                .ToArray();
        }

        public GuildMemberEntity[] GetGuildBlocked(AuthorizedUser user, Int32 guildId)
        {
            StudentEntity student = user.GetProfile(_database.Student);
            GuildEntity guild = _database.Guild.Get(guildId);
            student.EnsureIsGuildEditor(guild);

            return guild.Members
                .Where(m => m.MemberType == GuildMemberType.Blocked)
                .ToArray();
        }

        public void BlockGuildMember(AuthorizedUser user, Int32 guildId, Int32 memberId)
        {
            var guildDomain = new GuildDomain(_database.Guild.Get(guildId), _database, _githubUserDataService, _githubApiAccessor);
            GuildMemberEntity memberToKick = guildDomain.EnsureMemberCanRestrictPermissionForOther(user, memberId);

            memberToKick.Member.GuildLeftTime = DateTime.UtcNow.ToUniversalTime();
            memberToKick.MemberType = GuildMemberType.Blocked;
            _database.GuildMember.UpdateMember(memberToKick);
        }

        public void UnblockStudent(AuthorizedUser user, Int32 guildId, Int32 studentId)
        {
            StudentEntity student = user.GetProfile(_database.Student);
            GuildEntity guild = _database.Guild.Get(guildId);
            student.EnsureIsGuildEditor(guild);

            GuildMemberEntity member = guild.Members.Find(m => m.MemberId == studentId);

            if (member is null || member.MemberType != GuildMemberType.Blocked)
                throw new InnerLogicException($"Student is not blocked in guild! StudentId: {studentId} GuildId: {guildId}");

            _database.GuildMember.RemoveMember(guildId, studentId);
        }

        public void KickGuildMember(AuthorizedUser user, Int32 guildId, Int32 memberId)
        {
            var guildDomain = new GuildDomain(_database.Guild.Get(guildId), _database, _githubUserDataService, _githubApiAccessor);
            GuildMemberEntity memberToKick = guildDomain.EnsureMemberCanRestrictPermissionForOther(user, memberId);

            memberToKick.Member.GuildLeftTime = DateTime.UtcNow.ToUniversalTime();
            _database.GuildMember.RemoveMember(guildId, memberId);
        }

        public void AcceptRequest(AuthorizedUser user, Int32 guildId, Int32 studentId)
        {
            StudentEntity student = user.GetProfile(_database.Student);
            GuildEntity guild = _database.Guild.Get(guildId);
            student.EnsureIsGuildEditor(guild);

            GuildMemberEntity member = guild.Members.Find(m => m.MemberId == studentId);

            if (member is null || member.MemberType != GuildMemberType.Requested)
                throw InnerLogicException.Guild.RequestWasNotFound(studentId, guildId);

            member.MemberType = GuildMemberType.Member;

            _database.GuildMember.UpdateMember(member);
        }

        public void RejectRequest(AuthorizedUser user, Int32 guildId, Int32 studentId)
        {
            StudentEntity student = user.GetProfile(_database.Student);
            GuildEntity guild = _database.Guild.Get(guildId);
            student.EnsureIsGuildEditor(guild);

            GuildMemberEntity member = guild.Members.Find(m => m.MemberId == studentId);

            if (member is null || member.MemberType != GuildMemberType.Requested)
                throw InnerLogicException.Guild.RequestWasNotFound(studentId, guildId);

            _database.GuildMember.RemoveMember(guildId, studentId);
        }

        public GuildProfileDto Get(int id, int? userId)
        {
            return _database.Guild.Get(id)
                .To(g => new GuildDomain(g, _database, _githubUserDataService, _githubApiAccessor))
                .ToGuildProfileDto(userId);
        }
    }
}
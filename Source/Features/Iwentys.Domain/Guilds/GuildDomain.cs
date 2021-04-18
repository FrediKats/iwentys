using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.Guilds.Enums;
using Iwentys.Domain.Models;
using Iwentys.Domain.Services;

namespace Iwentys.Domain.Guilds
{
    public class GuildDomain
    {
        private readonly IGenericRepository<IwentysUser> _userRepository;
        private readonly IGenericRepository<GuildLastLeave> _guildLastLeaveRepository;
        private readonly IGithubUserApiAccessor _githubUserApiAccessor;

        public GuildDomain(
            Guild profile,
            IGenericRepository<IwentysUser> studentRepository,
            IGenericRepository<GuildLastLeave> guildLastLeaveRepository,
            IGithubUserApiAccessor githubUserApiAccessor)
        {
            Profile = profile;
            _userRepository = studentRepository;
            _guildLastLeaveRepository = guildLastLeaveRepository;
            _githubUserApiAccessor = githubUserApiAccessor;
        }

        public Guild Profile { get; }

        public async Task<UserMembershipState> GetUserMembershipState(IwentysUser user, Guild userGuild, GuildMember currentMembership)
        {
            GuildMemberType? userStatusInGuild = Profile.Members.Find(m => m.Member.Id == user.Id)?.MemberType;
            GuildLastLeave guildLastLeave = await GuildLastLeave.Get(user, _guildLastLeaveRepository);

            if (userStatusInGuild == GuildMemberType.Blocked)
                return UserMembershipState.Blocked;

            if (userGuild is not null &&
                userGuild.Id != Profile.Id)
                return UserMembershipState.Blocked;

            if (userGuild is not null &&
                userGuild.Id == Profile.Id)
                return UserMembershipState.Entered;

            if (currentMembership?.MemberType == GuildMemberType.Requested &&
                userStatusInGuild != GuildMemberType.Requested)
                return UserMembershipState.Blocked;

            if (currentMembership?.MemberType == GuildMemberType.Requested &&
                userStatusInGuild == GuildMemberType.Requested)
                return UserMembershipState.Requested;

            if (userGuild is null &&
                userStatusInGuild != GuildMemberType.Requested &&
                guildLastLeave.IsLeaveRestrictExpired())
                return UserMembershipState.Blocked;

            if (userGuild is null && Profile.HiringPolicy == GuildHiringPolicy.Open)
                return UserMembershipState.CanEnter;

            if (userGuild is null && Profile.HiringPolicy == GuildHiringPolicy.Close)
                return UserMembershipState.CanRequest;

            return UserMembershipState.Blocked;
        }

        public async Task<GuildMember> EnsureMemberCanRestrictPermissionForOther(AuthorizedUser user, int memberToKickId)
        {
            IwentysUser editorStudentAccount = await _userRepository.GetById(user.Id);
            editorStudentAccount.EnsureIsGuildMentor(Profile);

            GuildMember memberToKick = Profile.Members.Find(m => m.MemberId == memberToKickId);
            GuildMember editorMember = Profile.Members.Find(m => m.MemberId == user.Id) ?? throw new EntityNotFoundException(nameof(GuildMember));

            //TODO: check
            //if (memberToKick is null || !memberToKick.MemberType.IsMember())
            if (memberToKick is null)
                throw InnerLogicException.GuildExceptions.IsNotGuildMember(user.Id, Profile.Id);

            if (memberToKick.MemberType == GuildMemberType.Creator)
                throw InnerLogicException.GuildExceptions.StudentCannotBeBlocked(memberToKickId, Profile.Id);

            if (memberToKick.MemberType == GuildMemberType.Mentor && editorMember.MemberType == GuildMemberType.Mentor)
                throw InnerLogicException.GuildExceptions.StudentCannotBeBlocked(memberToKickId, Profile.Id);

            return memberToKick;
        }
    }
}
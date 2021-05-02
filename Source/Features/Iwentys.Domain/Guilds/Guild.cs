using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.GithubIntegration.Models;
using Iwentys.Domain.Guilds.Enums;
using Iwentys.Domain.Guilds.Models;

namespace Iwentys.Domain.Guilds
{
    public class Guild
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public string Bio { get; set; }
        public string ImageUrl { get; set; }
        public string TestTaskLink { get; private set; }

        public GuildHiringPolicy HiringPolicy { get; set; }
        public GuildType GuildType { get; private set; }

        public virtual List<GuildMember> Members { get; init; } = new List<GuildMember>();
        public virtual List<GuildPinnedProject> PinnedProjects { get; init; } = new List<GuildPinnedProject>();
        public virtual List<GuildTestTaskSolution> TestTasks { get; init; } = new List<GuildTestTaskSolution>();

        public static Guild Create(IwentysUser creator, Guild userCurrentGuild, GuildCreateRequestDto arguments)
        {
            if (userCurrentGuild is not null)
                throw new InnerLogicException("Student already in guild");

            var newGuild = new Guild
            {
                Bio = arguments.Bio,
                HiringPolicy = arguments.HiringPolicy,
                ImageUrl = arguments.LogoUrl,
                Title = arguments.Title,
                GuildType = GuildType.Pending
            };

            newGuild.Members.Add(new GuildMember(newGuild, creator, GuildMemberType.Creator));

            return newGuild;
        }

        public void Update(IwentysUser user, GuildUpdateRequestDto arguments)
        {
            GuildMentor mentor = EnsureIsGuildMentor(user);

            Bio = arguments.Bio ?? Bio;
            ImageUrl = arguments.LogoUrl ?? ImageUrl;
            TestTaskLink = arguments.TestTaskLink ?? TestTaskLink;
            HiringPolicy = arguments.HiringPolicy ?? HiringPolicy;

            if (arguments.HiringPolicy == GuildHiringPolicy.Open)
                foreach (GuildMember guildMember in Members.Where(guildMember => guildMember.MemberType == GuildMemberType.Requested))
                    guildMember.Approve(mentor);
        }

        public void Approve(SystemAdminUser admin)
        {
            if (GuildType == GuildType.Created)
                throw new InnerLogicException("Guild already approved");

            GuildType = GuildType.Created;
        }

        public List<GuildMemberImpactDto> GetImpact()
        {
            return Members.Select(member => new GuildMemberImpactDto(member)).ToList();
        }

        public GuildMentor EnsureIsGuildMentor(IwentysUser user)
        {
            GuildMember membership = Members.FirstOrDefault(m => m.MemberId == user.Id);

            if (membership is null)
                throw InnerLogicException.GuildExceptions.IsNotGuildMember(user.Id, Id);

            return new GuildMentor(user, this, membership.MemberType);
        }

        //TODO: rework
        public async Task<List<GuildMemberImpactDto>> GetMemberImpacts(IGithubUserApiAccessor githubUserApiAccessor)
        {
            //FYI: optimization is need
            var result = new List<GuildMemberImpactDto>();
            foreach (GuildMember member in Members)
            {
                ContributionFullInfo contributionFullInfo = await githubUserApiAccessor.FindUserContributionOrEmpty(member.Member);
                result.Add(new GuildMemberImpactDto(new IwentysUserInfoDto(member.Member), member.MemberType, contributionFullInfo));
            }

            return result;
        }

        public GuildMember EnterGuild(IwentysUser user, GuildMember guildMember, GuildLastLeave lastLeave)
        {
            if (GetUserMembershipState(user, guildMember, lastLeave) != UserMembershipState.CanEnter)
                throw new InnerLogicException($"Student unable to enter this guild! UserId: {user.Id} GuildId: {Id}");

            return new GuildMember(this, user, GuildMemberType.Member);
        }

        public GuildMember RequestEnterGuild(IwentysUser user, GuildMember guildMember, GuildLastLeave lastLeave)
        {
            if (GetUserMembershipState(user, guildMember, lastLeave) != UserMembershipState.CanRequest)
                throw new InnerLogicException($"Student unable to send request to this guild! UserId: {user.Id} GuildId: {Id}");

            return new GuildMember(this, user, GuildMemberType.Requested);
        }

        public UserMembershipState GetUserMembershipState(IwentysUser user, GuildMember currentMembership, GuildLastLeave guildLastLeave)
        {
            GuildMemberType? userStatusInGuild = Members?.Find(m => m.Member.Id == user.Id)?.MemberType;

            if (userStatusInGuild == GuildMemberType.Blocked)
                return UserMembershipState.Blocked;

            if (currentMembership is not null &&
                currentMembership.GuildId != Id)
                return UserMembershipState.Blocked;

            if (currentMembership is not null &&
                currentMembership.GuildId == Id)
                return UserMembershipState.Entered;

            if (currentMembership?.MemberType == GuildMemberType.Requested &&
                userStatusInGuild != GuildMemberType.Requested)
                return UserMembershipState.Blocked;

            if (currentMembership?.MemberType == GuildMemberType.Requested &&
                userStatusInGuild == GuildMemberType.Requested)
                return UserMembershipState.Requested;

            if (currentMembership is null &&
                userStatusInGuild != GuildMemberType.Requested &&
                (guildLastLeave?.IsLeaveRestrictExpired() ?? false))
                return UserMembershipState.Blocked;

            if (currentMembership is null && HiringPolicy == GuildHiringPolicy.Open)
                return UserMembershipState.CanEnter;

            if (currentMembership is null && HiringPolicy == GuildHiringPolicy.Close)
                return UserMembershipState.CanRequest;

            return UserMembershipState.Blocked;
        }

        public GuildMember EnsureMemberCanRestrictPermissionForOther(IwentysUser editorStudentAccount, int memberToKickId)
        {
            editorStudentAccount.EnsureIsGuildMentor(this);

            GuildMember memberToKick = Members.Find(m => m.MemberId == memberToKickId);
            GuildMember editorMember = Members.Find(m => m.MemberId == editorStudentAccount.Id) ?? throw new EntityNotFoundException(nameof(GuildMember));

            //TODO: check
            //if (memberToKick is null || !memberToKick.MemberType.IsMember())
            if (memberToKick is null)
                throw InnerLogicException.GuildExceptions.IsNotGuildMember(editorStudentAccount.Id, Id);

            if (memberToKick.MemberType == GuildMemberType.Creator)
                throw InnerLogicException.GuildExceptions.StudentCannotBeBlocked(memberToKickId, Id);

            if (memberToKick.MemberType == GuildMemberType.Mentor && editorMember.MemberType == GuildMemberType.Mentor)
                throw InnerLogicException.GuildExceptions.StudentCannotBeBlocked(memberToKickId, Id);

            return memberToKick;
        }

        public void RemoveMember(IwentysUser mentor, IwentysUser memberToRemove, GuildLastLeave guildLastLeave)
        {
            EnsureMemberCanRestrictPermissionForOther(mentor, memberToRemove.Id);

            GuildMember guildMember = Members.Single(gm => gm.MemberId == memberToRemove.Id);
            if (guildMember.MemberType == GuildMemberType.Creator)
                throw InnerLogicException.GuildExceptions.CreatorCannotLeave(memberToRemove.Id, Id);

            guildLastLeave.UpdateLeave();
            Members.Remove(guildMember);
        }
    }
}
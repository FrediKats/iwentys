using System;
using System.Collections.Generic;
using System.Linq;
using Iwentys.Core.GithubIntegration;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Transferable.Students;
using Iwentys.Models.Types.Guilds;
using Octokit;

namespace Iwentys.Core.DomainModel.Guilds
{
    public class GuildDomain
    {
        public GuildEntity Profile { get; }

        private readonly DatabaseAccessor _dbAccessor;
        private readonly IGithubUserDataService _githubUserDataService;
        private readonly IGithubApiAccessor _apiAccessor;

        public GuildDomain(GuildEntity profile, DatabaseAccessor dbAccessor, IGithubUserDataService githubUserDataService, IGithubApiAccessor apiAccessor)
        {
            Profile = profile;
            _dbAccessor = dbAccessor;
            _githubUserDataService = githubUserDataService;
            _apiAccessor = apiAccessor;
        }

        public GuildProfileShortInfoDto ToGuildProfileShortInfoDto()
        {
            return new GuildProfileShortInfoDto(Profile);
        }

        public GuildProfileDto ToGuildProfileDto(int? userId = null)
        {
            GuildMemberLeaderBoard dashboard = GetMemberDashboard();

            var info = new GuildProfileDto(Profile)
            {
                Leader = Profile.Members.Single(m => m.MemberType == GuildMemberType.Creator).Member.To(s => new StudentPartialProfileDto(s)),
                MemberLeaderBoard = dashboard,
                Rating = dashboard.TotalRate,
                PinnedRepositories = Profile.PinnedProjects.SelectToList(p => _githubUserDataService.GetCertainRepository(p.RepositoryOwner, p.RepositoryName)),
                Achievements = Profile.Achievements.SelectToList(AchievementInfoDto.Wrap),
                TestTasks = Profile.TestTasks.SelectToList(GuildTestTaskInfoDto.Wrap)
            };

            if (userId != null && Profile.Members.Any(m => m.MemberId == userId))
                info.Tribute = _dbAccessor.TributeRepository.ReadStudentActiveTribute(Profile.Id, userId.Value)?.To(ActiveTributeDto.Create);
            if (userId != null)
                info.UserMembershipState = GetUserMembershipState(userId.Value);

            return info;
        }

        public GuildProfilePreviewDto ToGuildProfilePreviewDto()
        {
            return new GuildProfilePreviewDto(Profile)
            {
                Leader = Profile.Members.Single(m => m.MemberType == GuildMemberType.Creator).Member.To(s => new StudentPartialProfileDto(s)),
                Rating = GetMemberDashboard().TotalRate
            };
        }

        public GuildMemberLeaderBoard GetMemberDashboard()
        {
            List<GuildMemberImpact> members = Profile
                .Members
                .Select(m => m.Member.GithubUsername)
                .Where(gh => gh != null)
                .Select(ghName =>
                {
                    var totalImpact = _githubUserDataService.GetUserDataByUsername(ghName)?.ContributionFullInfo.Total;
                    if (totalImpact == null)
                        return new GuildMemberImpact(ghName, 0);
                    return new GuildMemberImpact(ghName, totalImpact.Value);
                })
                .ToList();

            return new GuildMemberLeaderBoard
            {
                TotalRate = members.Sum(m => m.TotalRate),
                MembersImpact = members,
                Members = Profile.Members.SelectToList(m => new StudentPartialProfileDto(m.Member))
            };
        }

        
        public UserMembershipState GetUserMembershipState(Int32 userId)
        {
            StudentEntity user = _dbAccessor.Student.Get(userId);
            GuildEntity userGuild = _dbAccessor.GuildRepository.ReadForStudent(user.Id);
            GuildMemberType? userStatusInGuild = Profile.Members.Find(m => m.Member.Id == user.Id)?.MemberType;

            if (userStatusInGuild == GuildMemberType.Blocked)
                return UserMembershipState.Blocked;

            if (userGuild != null && 
                userGuild.Id != Profile.Id)
                return UserMembershipState.Blocked;

            if (userGuild != null && 
                userGuild.Id == Profile.Id)
                return UserMembershipState.Entered;

            if (_dbAccessor.GuildRepository.IsStudentHaveRequest(userId) &&
                userStatusInGuild != GuildMemberType.Requested)
                return UserMembershipState.Blocked;

            if (_dbAccessor.GuildRepository.IsStudentHaveRequest(userId) &&
                userStatusInGuild == GuildMemberType.Requested)
                return UserMembershipState.Requested;

            if (userGuild is null &&
                userStatusInGuild != GuildMemberType.Requested &&
                DateTime.UtcNow < user.GuildLeftTime.AddHours(24))
                return UserMembershipState.Blocked;

            if (userGuild is null && Profile.HiringPolicy == GuildHiringPolicy.Open)
                return UserMembershipState.CanEnter;

            if (userGuild is null && Profile.HiringPolicy == GuildHiringPolicy.Close)
                return UserMembershipState.CanRequest;

            return UserMembershipState.Blocked;
        }

        //TODO: use in daemon
        public GuildDomain UpdateGuildFromGithub()
        {
            Organization organizationInfo = _apiAccessor.FindOrganizationInfo(Profile.Title);
            if (organizationInfo != null)
            {
                //TODO: need to fix after https://github.com/octokit/octokit.net/pull/2239
                //_profile.Bio = organizationInfo.Bio;
                Profile.LogoUrl = organizationInfo.Url;
                _dbAccessor.GuildRepository.Update(Profile);
            }

            return this;
        }

        public GuildMemberEntity EnsureMemberCanRestrictPermissionForOther(AuthorizedUser editor, int memberToKickId)
        {
            StudentEntity editorStudentAccount = editor.GetProfile(_dbAccessor.Student);
            editorStudentAccount.EnsureIsGuildEditor(Profile);

            GuildMemberEntity memberToKick = Profile.Members.Find(m => m.MemberId == memberToKickId);
            GuildMemberEntity editorMember = Profile.Members.Find(m => m.MemberId == editor.Id) ?? throw new EntityNotFoundException(nameof(GuildMemberEntity));

            if (memberToKick is null || !memberToKick.MemberType.IsMember())
                throw InnerLogicException.Guild.IsNotGuildMember(editor.Id, Profile.Id);

            if (memberToKick.MemberType == GuildMemberType.Creator)
                throw InnerLogicException.Guild.StudentCannotBeBlocked(memberToKickId, Profile.Id);

            if (memberToKick.MemberType == GuildMemberType.Mentor && editorMember.MemberType == GuildMemberType.Mentor)
                throw InnerLogicException.Guild.StudentCannotBeBlocked(memberToKickId, Profile.Id);

            return memberToKick;
        }
    }
}
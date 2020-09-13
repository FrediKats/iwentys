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
        private readonly GuildEntity _profile;
        private readonly DatabaseAccessor _dbAccessor;
        private readonly IGithubUserDataService _githubUserDataService;
        private readonly IGithubApiAccessor _apiAccessor;

        public GuildDomain(GuildEntity profile, DatabaseAccessor dbAccessor, IGithubUserDataService githubUserDataService, IGithubApiAccessor apiAccessor)
        {
            _profile = profile;
            _dbAccessor = dbAccessor;
            _githubUserDataService = githubUserDataService;
            _apiAccessor = apiAccessor;
        }

        public GuildProfileShortInfoDto ToGuildProfileShortInfoDto()
        {
            return new GuildProfileShortInfoDto(_profile);
        }

        public GuildProfileDto ToGuildProfileDto(int? userId = null)
        {
            GuildMemberLeaderBoard dashboard = GetMemberDashboard();

            var info = new GuildProfileDto(_profile)
            {
                Leader = _profile.Members.Single(m => m.MemberType == GuildMemberType.Creator).Member.To(s => new StudentPartialProfileDto(s)),
                MemberLeaderBoard = dashboard,
                Rating = dashboard.TotalRate,
                PinnedRepositories = _profile.PinnedProjects.SelectToList(p => _githubUserDataService.GetCertainRepository(p.RepositoryOwner, p.RepositoryName)),
                Achievements = _profile.Achievements.SelectToList(AchievementInfoDto.Wrap)
            };

            if (userId != null && _profile.Members.Any(m => m.MemberId == userId))
                info.Tribute = _dbAccessor.TributeRepository.ReadStudentActiveTribute(_profile.Id, userId.Value)?.To(ActiveTributeDto.Create);
            if (userId != null)
                info.UserMembershipState = GetUserMembershipState(userId.Value);

            return info;
        }

        public GuildProfilePreviewDto ToGuildProfilePreviewDto()
        {
            return new GuildProfilePreviewDto(_profile)
            {
                Leader = _profile.Members.Single(m => m.MemberType == GuildMemberType.Creator).Member.To(s => new StudentPartialProfileDto(s)),
                Rating = GetMemberDashboard().TotalRate
            };
        }

        public GuildMemberLeaderBoard GetMemberDashboard()
        {
            List<GuildMemberImpact> members = _profile
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
                Members = _profile.Members.SelectToList(m => new StudentPartialProfileDto(m.Member))
            };
        }

        
        public UserMembershipState GetUserMembershipState(Int32 userId)
        {
            Student user = _dbAccessor.Student.Get(userId);
            GuildEntity userGuild = _dbAccessor.GuildRepository.ReadForStudent(user.Id);
            GuildMemberType? userStatusInGuild = _profile.Members.Find(m => m.Member.Id == user.Id)?.MemberType;

            if (userStatusInGuild == GuildMemberType.Blocked)
                return UserMembershipState.Blocked;

            if (userGuild != null && 
                userGuild.Id != _profile.Id)
                return UserMembershipState.Blocked;

            if (userGuild != null && 
                userGuild.Id == _profile.Id)
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

            if (userGuild is null && _profile.HiringPolicy == GuildHiringPolicy.Open)
                return UserMembershipState.CanEnter;

            if (userGuild is null && _profile.HiringPolicy == GuildHiringPolicy.Close)
                return UserMembershipState.CanRequest;

            return UserMembershipState.Blocked;
        }

        //TODO: use in daemon
        public GuildDomain UpdateGuildFromGithub()
        {
            Organization organizationInfo = _apiAccessor.FindOrganizationInfo(_profile.Title);
            if (organizationInfo != null)
            {
                //TODO: need to fix after https://github.com/octokit/octokit.net/pull/2239
                //_profile.Bio = organizationInfo.Bio;
                _profile.LogoUrl = organizationInfo.Url;
                _dbAccessor.GuildRepository.Update(_profile);
            }

            return this;
        }

        public GuildMember EnsureMemberCanRestrictPermissionForOther(AuthorizedUser editor, int memberToKickId)
        {
            Student editorStudentAccount = editor.GetProfile(_dbAccessor.Student);
            editorStudentAccount.EnsureIsGuildEditor(_profile);

            GuildMember memberToKick = _profile.Members.Find(m => m.MemberId == memberToKickId);
            GuildMember editorMember = _profile.Members.Find(m => m.MemberId == editor.Id) ?? throw new EntityNotFoundException(nameof(GuildMember));

            if (memberToKick is null || !memberToKick.MemberType.IsMember())
                throw InnerLogicException.Guild.IsNotGuildMember(editor.Id, _profile.Id);

            if (memberToKick.MemberType == GuildMemberType.Creator)
                throw InnerLogicException.Guild.StudentCannotBeBlocked(memberToKickId, _profile.Id);

            if (memberToKick.MemberType == GuildMemberType.Mentor && editorMember.MemberType == GuildMemberType.Mentor)
                throw InnerLogicException.Guild.StudentCannotBeBlocked(memberToKickId, _profile.Id);

            return memberToKick;
        }
    }
}
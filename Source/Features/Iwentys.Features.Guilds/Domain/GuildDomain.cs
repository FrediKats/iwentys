using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Iwentys.Common.Tools;
using Iwentys.Features.Achievements.ViewModels;
using Iwentys.Features.GithubIntegration;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Guilds.Repositories;
using Iwentys.Features.Guilds.ViewModels.Guilds;
using Iwentys.Features.StudentFeature;
using Iwentys.Features.StudentFeature.Repositories;
using Iwentys.Integrations.GithubIntegration;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Github;
using Iwentys.Models.Transferable.Students;
using Octokit;

namespace Iwentys.Features.Guilds.Domain
{
    public class GuildDomain
    {
        public GuildEntity Profile { get; }

        private readonly GithubUserDataService _githubUserDataService;
        private readonly IGithubApiAccessor _apiAccessor;
        private readonly IStudentRepository _studentRepository;
        private readonly IGuildRepository _guildRepository;
        private readonly IGuildMemberRepository _guildMemberRepository;
        private readonly IGuildTributeRepository _guildTributeRepository;

        public GuildDomain(
            GuildEntity profile,
            GithubUserDataService githubUserDataService,
            IGithubApiAccessor apiAccessor,
            GuildRepositoriesScope repositoriesScope)
        {
            Profile = profile;
            _githubUserDataService = githubUserDataService;
            _apiAccessor = apiAccessor;
            _studentRepository = repositoriesScope.Student;
            _guildRepository = repositoriesScope.Guild;
            _guildMemberRepository = repositoriesScope.GuildMember;
            _guildTributeRepository = repositoriesScope.GuildTribute;
        }

        public GuildDomain(
            GuildEntity profile,
            GithubUserDataService githubUserDataService,
            IGithubApiAccessor apiAccessor,
            IStudentRepository studentRepository,
            IGuildRepository guildRepository,
            IGuildMemberRepository guildMemberRepository,
            IGuildTributeRepository guildTributeRepository)
        {
            Profile = profile;
            _githubUserDataService = githubUserDataService;
            _apiAccessor = apiAccessor;
            _studentRepository = studentRepository;
            _guildRepository = guildRepository;
            _guildMemberRepository = guildMemberRepository;
            _guildTributeRepository = guildTributeRepository;
        }

        public GuildProfileShortInfoDto ToGuildProfileShortInfoDto()
        {
            return new GuildProfileShortInfoDto(Profile);
        }

        public async Task<GuildProfileDto> ToGuildProfileDto(int? userId = null)
        {
            GuildMemberLeaderBoard dashboard = GetMemberDashboard();

            var info = new GuildProfileDto(Profile)
            {
                Leader = Profile.Members.Single(m => m.MemberType == GuildMemberType.Creator).Member.To(s => new StudentPartialProfileDto(s)),
                MemberLeaderBoard = dashboard,
                Rating = dashboard.TotalRate,
                PinnedRepositories = Profile.PinnedProjects.SelectToList(p => _githubUserDataService.GetCertainRepository(p.RepositoryOwner, p.RepositoryName)),
                Achievements = Profile.Achievements.SelectToList(AchievementInfoDto.Wrap),
                TestTasks = Profile.TestTasks.SelectToList(GuildTestTaskInfoResponse.Wrap)
            };

            if (userId != null && Profile.Members.Any(m => m.MemberId == userId))
                info.Tribute = _guildTributeRepository.ReadStudentActiveTribute(Profile.Id, userId.Value)?.To(ActiveTributeResponse.Create);
            if (userId != null)
                info.UserMembershipState = await GetUserMembershipState(userId.Value);

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

        public List<GithubUserEntity> GetGithubUserData()
        {
            return Profile
                .Members
                .Select(m => m.Member.GithubUsername)
                .Where(gh => gh != null)
                .ToList()
                .Select(ghName => _githubUserDataService.FindByUsername(ghName).Result)
                .Where(userData => userData != null)
                .ToList();
        }

        public GuildMemberLeaderBoard GetMemberDashboard()
        {
            List<GuildMemberImpact> members = GetGithubUserData().SelectToList(userData => new GuildMemberImpact(userData));

            return new GuildMemberLeaderBoard
            {
                TotalRate = members.Sum(m => m.TotalRate),
                MembersImpact = members,
                Members = Profile.Members.SelectToList(m => new StudentPartialProfileDto(m.Member))
            };
        }

        public async Task<UserMembershipState> GetUserMembershipState(Int32 userId)
        {
            StudentEntity user = await _studentRepository.GetAsync(userId);
            GuildEntity userGuild = _guildRepository.ReadForStudent(user.Id);
            GuildMemberType? userStatusInGuild = Profile.Members.Find(m => m.Member.Id == user.Id)?.MemberType;

            if (userStatusInGuild == GuildMemberType.Blocked)
                return UserMembershipState.Blocked;

            if (userGuild != null &&
                userGuild.Id != Profile.Id)
                return UserMembershipState.Blocked;

            if (userGuild != null &&
                userGuild.Id == Profile.Id)
                return UserMembershipState.Entered;

            if (_guildMemberRepository.IsStudentHaveRequest(userId) &&
                userStatusInGuild != GuildMemberType.Requested)
                return UserMembershipState.Blocked;

            if (_guildMemberRepository.IsStudentHaveRequest(userId) &&
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
                _guildRepository.UpdateAsync(Profile);
            }

            return this;
        }

        public async Task<GuildMemberEntity> EnsureMemberCanRestrictPermissionForOther(AuthorizedUser editor, int memberToKickId)
        {
            StudentEntity editorStudentAccount = await editor.GetProfile(_studentRepository);
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
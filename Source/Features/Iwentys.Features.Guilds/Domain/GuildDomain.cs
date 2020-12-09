using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Iwentys.Common.Tools;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.GithubIntegration.Services;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Guilds.Models.Guilds;
using Iwentys.Features.Guilds.Repositories;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Models;
using Iwentys.Features.Students.Repositories;

namespace Iwentys.Features.Guilds.Domain
{
    public class GuildDomain
    {
        public GuildEntity Profile { get; }

        private readonly GithubIntegrationService _githubIntegrationService;
        private readonly IStudentRepository _studentRepository;
        private readonly IGuildRepository _guildRepository;
        private readonly IGuildMemberRepository _guildMemberRepository;

        public GuildDomain(
            GuildEntity profile,
            GithubIntegrationService githubIntegrationService,
            IStudentRepository studentRepository,
            IGuildRepository guildRepository,
            IGuildMemberRepository guildMemberRepository)
        {
            Profile = profile;
            _githubIntegrationService = githubIntegrationService;
            _studentRepository = studentRepository;
            _guildRepository = guildRepository;
            _guildMemberRepository = guildMemberRepository;
        }

        public async Task<ExtendedGuildProfileWithMemberDataDto> ToExtendedGuildProfileDto(int? userId = null)
        {
            var info = new ExtendedGuildProfileWithMemberDataDto(Profile)
            {
                Leader = Profile.Members.Single(m => m.MemberType == GuildMemberType.Creator).Member.To(s => new StudentInfoDto(s)),
                PinnedRepositories = Profile.PinnedProjects.SelectToList(p => _githubIntegrationService.GetCertainRepository(p.RepositoryOwner, p.RepositoryName)),
            };

            if (userId != null)
                info.UserMembershipState = await GetUserMembershipState(userId.Value);

            return info;
        }

        public List<GithubUserEntity> GetGithubUserData()
        {
            return Profile
                .Members
                .Select(m => m.Member.GithubUsername)
                .Where(gh => gh != null)
                .ToList()
                .Select(ghName => _githubIntegrationService.FindByUsername(ghName).Result)
                .Where(userData => userData != null)
                .ToList();
        }

        public GuildMemberLeaderBoardDto GetMemberDashboard()
        {
            List<GuildMemberImpactDto> members = GetGithubUserData().SelectToList(userData => new GuildMemberImpactDto(userData));

            return new GuildMemberLeaderBoardDto(
                members.Sum(m => m.TotalRate),
                Profile.Members.SelectToList(m => new StudentInfoDto(m.Member)),
                members);
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
        //public GuildDomain UpdateGuildFromGithub()
        //{
        //    Organization organizationInfo = _apiAccessor.FindOrganizationInfo(Profile.Title);
        //    if (organizationInfo != null)
        //    {
        //        //TODO: need to fix after https://github.com/octokit/octokit.net/pull/2239
        //        //_profile.Bio = organizationInfo.Bio;
        //        Profile.LogoUrl = organizationInfo.Url;
        //        _guildRepository.UpdateAsync(Profile);
        //    }

        //    return this;
        //}

        public async Task<GuildMemberEntity> EnsureMemberCanRestrictPermissionForOther(AuthorizedUser editor, int memberToKickId)
        {
            StudentEntity editorStudentAccount = await editor.GetProfile(_studentRepository);
            editorStudentAccount.EnsureIsGuildEditor(Profile);

            GuildMemberEntity memberToKick = Profile.Members.Find(m => m.MemberId == memberToKickId);
            GuildMemberEntity editorMember = Profile.Members.Find(m => m.MemberId == editor.Id) ?? throw new EntityNotFoundException(nameof(GuildMemberEntity));

            //TODO: check
            //if (memberToKick is null || !memberToKick.MemberType.IsMember())
            if (memberToKick is null)
                throw InnerLogicException.Guild.IsNotGuildMember(editor.Id, Profile.Id);

            if (memberToKick.MemberType == GuildMemberType.Creator)
                throw InnerLogicException.Guild.StudentCannotBeBlocked(memberToKickId, Profile.Id);

            if (memberToKick.MemberType == GuildMemberType.Mentor && editorMember.MemberType == GuildMemberType.Mentor)
                throw InnerLogicException.Guild.StudentCannotBeBlocked(memberToKickId, Profile.Id);

            return memberToKick;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Common.Tools;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.AccountManagement.Entities;
using Iwentys.Features.AccountManagement.Models;
using Iwentys.Features.GithubIntegration.Services;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Guilds.Models;
using Iwentys.Features.Guilds.Repositories;

namespace Iwentys.Features.Guilds.Domain
{
    public class GuildDomain
    {
        public Guild Profile { get; }

        private readonly GithubIntegrationService _githubIntegrationService;
        private readonly IGenericRepository<IwentysUser> _userRepository;
        private readonly IGenericRepository<GuildMember> _guildMemberRepositoryNew;

        public GuildDomain(
            Guild profile,
            GithubIntegrationService githubIntegrationService,
            IGenericRepository<IwentysUser> studentRepository,
            IGenericRepository<GuildMember> guildMemberRepositoryNew)
        {
            Profile = profile;
            _githubIntegrationService = githubIntegrationService;
            _userRepository = studentRepository;
            _guildMemberRepositoryNew = guildMemberRepositoryNew;
        }

        public async Task<ExtendedGuildProfileWithMemberDataDto> ToExtendedGuildProfileDto(int? userId = null)
        {
            var info = new ExtendedGuildProfileWithMemberDataDto(Profile)
            {
                Leader = Profile.Members.Single(m => m.MemberType == GuildMemberType.Creator).Member.To(s => new IwentysUserInfoDto(s)),
                //TODO: Make method in Github service for getting projects for guild
                PinnedRepositories = Profile.PinnedProjects.SelectToList(p => _githubIntegrationService.GetRepository(p.RepositoryOwner, p.RepositoryName).Result),
            };

            if (userId is not null)
                info.UserMembershipState = await GetUserMembershipState(userId.Value);

            return info;
        }

        public async Task<List<GuildMemberImpactDto>> GetMemberImpacts()
        {
            //TODO: move to SQL
            List<GuildMemberImpactDto> result = new List<GuildMemberImpactDto>();
            foreach (var member in Profile.Members)
            {
                var contributionFullInfo = await _githubIntegrationService.FindUserContributionOrEmpty(member.Member);
                result.Add(new GuildMemberImpactDto(new IwentysUserInfoDto(member.Member), member.MemberType, contributionFullInfo));
            }

            return result;
        }

        public async Task<GuildMemberLeaderBoardDto> GetMemberDashboard()
        {
            List<GuildMemberImpactDto> members = await GetMemberImpacts();
            return new GuildMemberLeaderBoardDto(members);
        }

        public async Task<UserMembershipState> GetUserMembershipState(Int32 userId)
        {
            IwentysUser user = await _userRepository.GetByIdAsync(userId);
            Guild userGuild = _guildMemberRepositoryNew.ReadForStudent(user.Id);
            GuildMemberType? userStatusInGuild = Profile.Members.Find(m => m.Member.Id == user.Id)?.MemberType;

            if (userStatusInGuild == GuildMemberType.Blocked)
                return UserMembershipState.Blocked;

            if (userGuild is not null &&
                userGuild.Id != Profile.Id)
                return UserMembershipState.Blocked;

            if (userGuild is not null &&
                userGuild.Id == Profile.Id)
                return UserMembershipState.Entered;

            if (_guildMemberRepositoryNew.IsStudentHaveRequest(userId) &&
                userStatusInGuild != GuildMemberType.Requested)
                return UserMembershipState.Blocked;

            if (_guildMemberRepositoryNew.IsStudentHaveRequest(userId) &&
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
        //    if (organizationInfo is not null)
        //    {
        //        //TODO: need to fix after https://github.com/octokit/octokit.net/pull/2239
        //        //_profile.Bio = organizationInfo.Bio;
        //        Profile.LogoUrl = organizationInfo.Url;
        //        _guildRepository.UpdateAsync(Profile);
        //    }

        //    return this;
        //}

        public async Task<GuildMember> EnsureMemberCanRestrictPermissionForOther(AuthorizedUser user, int memberToKickId)
        {
            IwentysUser editorStudentAccount = await _userRepository.GetByIdAsync(user.Id);
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
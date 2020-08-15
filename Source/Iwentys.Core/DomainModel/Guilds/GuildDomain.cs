using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Transferable.Students;
using Iwentys.Models.Types.Guilds;

namespace Iwentys.Core.DomainModel.Guilds
{
    public class GuildDomain
    {
        private readonly Guild _profile;
        private readonly DatabaseAccessor _dbAccessor;
        private readonly IGithubUserDataService _githubUserDataService;

        public GuildDomain(Guild profile, DatabaseAccessor dbAccessor, IGithubUserDataService githubUserDataService)
        {
            _profile = profile;
            _dbAccessor = dbAccessor;
            _githubUserDataService = githubUserDataService;
        }

        public GuildProfileShortInfoDto ToGuildProfileShortInfoDto()
        {
            return new GuildProfileShortInfoDto
            {
                Id = _profile.Id,
                Bio = _profile.Bio,
                HiringPolicy = _profile.HiringPolicy,
                LogoUrl = _profile.LogoUrl,
                Title = _profile.Title,
            };
        }

        public GuildProfileDto ToGuildProfileDto(int? userId = null)
        {
            var info = new GuildProfileDto
            {
                Id = _profile.Id,
                Bio = _profile.Bio,
                HiringPolicy = _profile.HiringPolicy,
                LogoUrl = _profile.LogoUrl,
                Title = _profile.Title,
                Leader = _profile.Members.Single(m => m.MemberType == GuildMemberType.Creator).Member.To(s => new StudentPartialProfileDto(s)),
                MemberLeaderBoard = GetMemberDashboard(),
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
            var info = new GuildProfilePreviewDto()
            {
                Id = _profile.Id,
                Title = _profile.Title,
                LogoUrl = _profile.LogoUrl,
                Leader = _profile.Members.Single(m => m.MemberType == GuildMemberType.Creator).Member,
                Rating = GetMemberDashboard().TotalRate
            };

            return info;
        }

        private GuildMemberLeaderBoard GetMemberDashboard()
        {
            List<GuildMemberImpact> members = _profile
                .Members
                .Select(m => m.Member.GithubUsername)
                .Select(ghName => new GuildMemberImpact(ghName, _githubUserDataService.GetUserDataByUsername(ghName).ContributionFullInfo.Total))
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
            Guild userGuild = _dbAccessor.GuildRepository.ReadForStudent(user.Id);
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
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Iwentys.Core.GithubIntegration;
using Iwentys.Database.Repositories;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Types.Guilds;

namespace Iwentys.Core.DomainModel.Guilds
{
    public class GuildDomain
    {
        private readonly Guild _profile;
        private readonly ITributeRepository _tributeRepository;
        private readonly IGuildRepository _guildRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IGithubApiAccessor _apiAccessor;

        public GuildDomain(Guild profile, ITributeRepository tributeRepository, IGuildRepository guildRepository,
            IStudentRepository studentRepository, IGithubApiAccessor apiAccessor)
        {
            _profile = profile;
            _tributeRepository = tributeRepository;
            _guildRepository = guildRepository;
            _studentRepository = studentRepository;
            _apiAccessor = apiAccessor;
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
                Totem = _profile.Totem,
                Leader = _profile.Members.Single(m => m.MemberType == GuildMemberType.Creator).Member,
                MemberLeaderBoard = GetMemberDashboard(),
                PinnedRepositories = _profile.PinnedProjects.SelectToList(p => _apiAccessor.GetRepository(p.RepositoryOwner, p.RepositoryName)),
                
            };

            if (userId != null && _profile.Members.Any(m => m.MemberId == userId))
                info.Tribute = _tributeRepository.ReadStudentActiveTribute(_profile.Id, userId.Value)?.To(ActiveTributeDto.Create);
            if (userId != null)
                info.UserMembershipState = GetUserMembershipState(userId.Value);

            return info;
        }

        private GuildMemberLeaderBoard GetMemberDashboard()
        {
            List<(string ghName, int Total)> members = _profile
                .Members
                .Select(m => m.Member.GithubUsername)
                .Select(ghName => (ghName, _apiAccessor.GetUserActivity(ghName).Total))
                .ToList();

            return new GuildMemberLeaderBoard
            {
                TotalRate = members.Sum(m => m.Total),
                MembersImpact = members,
                Members = _profile.Members.SelectToList(m => m.Member)
            };
        }

        
        public UserMembershipState GetUserMembershipState(Int32 userId)
        {
            Student user = _studentRepository.Get(userId);
            Guild userGuild = _guildRepository.ReadForStudent(user.Id);
            GuildMemberType? userStatusInGuild = _profile.Members.Find(m => m.Member.Id == user.Id)?.MemberType;

            if (userStatusInGuild == GuildMemberType.Blocked)
                return UserMembershipState.Blocked;

            if (userGuild != null && 
                userGuild.Id != _profile.Id)
                return UserMembershipState.Blocked;

            if (userGuild != null && 
                userGuild.Id == _profile.Id)
                return UserMembershipState.Entered;

            if (_guildRepository.IsStudentHaveRequest(userId) && 
                userStatusInGuild != GuildMemberType.Requested)
                return UserMembershipState.Blocked;

            if (_guildRepository.IsStudentHaveRequest(userId) && 
                userStatusInGuild == GuildMemberType.Requested)
                return UserMembershipState.Requested;

            if (userGuild is null &&
                userStatusInGuild != GuildMemberType.Requested &&
                DateTime.Now.ToUniversalTime() < user.GuildLeftTime.AddHours(24))
                return UserMembershipState.Blocked;

            if (userGuild is null && _profile.HiringPolicy == GuildHiringPolicy.Open)
                return UserMembershipState.CanEnter;

            if (userGuild is null && _profile.HiringPolicy == GuildHiringPolicy.Close)
                return UserMembershipState.CanRequest;

            return UserMembershipState.Blocked;
        }
    }
}
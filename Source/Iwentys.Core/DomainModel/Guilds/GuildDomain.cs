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

            info.UserMembershipState = GetUserCapabilityInGuild(userId);

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

        
        public UserMembershipState GetUserCapabilityInGuild(Int32? userId)
        {
            if (userId is null)
                return UserMembershipState.Blocked;
            Student user = _studentRepository.Get(userId.Value);

            Guild userGuild = _guildRepository.ReadForStudent(user.Id);
            if(_guildRepository.IsStudentHaveRequest(userId.Value))
            {
                if (_profile.Members
                        .Find(m => m.Member.Id == user.Id && m.MemberType == GuildMemberType.Requested) != null)
                    return UserMembershipState.Requested;
                return UserMembershipState.Blocked;
            }
            if (userGuild is null)
            {
                if (_profile.Members.Find(m => m.Member.Id == user.Id && m.MemberType == GuildMemberType.Blocked) != null)
                    return UserMembershipState.Blocked;
                if (DateTime.Now < user.GuildLeftTime.AddHours(24))
                    return UserMembershipState.Blocked;

                return _profile.HiringPolicy == GuildHiringPolicy.Open ? UserMembershipState.CanEnter : UserMembershipState.CanRequest;
            }

            if (userGuild.Id == _profile.Id)
            {
                return UserMembershipState.Entered;
            }
            else 
            {
                return UserMembershipState.Blocked;
            }
        }
    }
}
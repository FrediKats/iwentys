using System.Collections.Generic;
using System.Linq;
using Iwentys.Core.GithubIntegration;
using Iwentys.Database.Repositories.Abstractions;
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
        private readonly IGithubApiAccessor _apiAccessor;

        public GuildDomain(Guild profile, ITributeRepository tributeRepository, IGithubApiAccessor apiAccessor)
        {
            _profile = profile;
            _tributeRepository = tributeRepository;
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

            return info;
        }

        private GuildMemberLeaderBoard GetMemberDashboard()
        {
            List<MemberImpact> members = _profile
                .Members
                .Select(m => m.Member.GithubUsername)
                .Select(ghName => new MemberImpact(ghName, _apiAccessor.GetUserActivity(ghName).Total))
                .ToList();

            return new GuildMemberLeaderBoard
            {
                TotalRate = members.Sum(m => m.TotalRate),
                MembersImpact = members,
                Members = _profile.Members.SelectToList(m => m.Member)
            };
        }
    }
}
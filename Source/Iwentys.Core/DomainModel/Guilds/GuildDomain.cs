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
                Members = _profile.Members.Select(m => m.Member).ToList(),
                PinnedRepositories = _profile.PinnedProjects.SelectToList(p => _apiAccessor.GetRepository(p.RepositoryOwner, p.RepositoryName))
            };

            if (userId != null && info.Members.Any(m => m.Id == userId))
                info.Tribute = ActiveTributeDto.Create(_tributeRepository.ReadStudentActiveTribute(_profile.Id, userId.Value));

            return info;
        }

    }
}
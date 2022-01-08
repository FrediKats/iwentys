using Iwentys.Common;
using Iwentys.Domain.Guilds;
using Iwentys.Infrastructure.Application.Controllers.GithubIntegration;

namespace Iwentys.Guilds
{
    public static class TournamentDomainHelper
    {
        public static ITournamentDomain WrapToDomain(
            this Tournament tournament,
            GithubIntegrationService githubIntegrationService)
        {
            return tournament.Type switch
            {
                TournamentType.CodeMarathon => new CodeMarathonTournamentDomain(tournament, githubIntegrationService.User),
                _ => throw InnerLogicException.NotSupportedEnumValue(tournament.Type)
            };
        }
    }
}
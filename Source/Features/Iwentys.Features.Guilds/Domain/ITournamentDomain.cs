using Iwentys.Common.Exceptions;
using Iwentys.Features.GithubIntegration;
using Iwentys.Features.GithubIntegration.Services;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Guilds.Models.Tournaments;

namespace Iwentys.Features.Guilds.Domain
{
    public interface ITournamentDomain
    {
        TournamentLeaderboardDto GetLeaderboard();
    }

    public static class TournamentDomainHelper
    {
        public static ITournamentDomain WrapToDomain(this TournamentEntity tournament, IGithubApiAccessor githubApiAccessor, GithubIntegrationService githubIntegrationService)
        {
            return tournament.Type switch
            {
                TournamentType.CodeMarathon => new CodeMarathonTournament(tournament, githubApiAccessor, githubIntegrationService),
                _ => throw InnerLogicException.NotSupportedEnumValue(tournament.Type)
            };
        }
    }
}
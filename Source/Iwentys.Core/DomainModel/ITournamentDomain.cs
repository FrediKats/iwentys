using Iwentys.Common.Exceptions;
using Iwentys.Core.Services;
using Iwentys.Database.Context;
using Iwentys.Integrations.GithubIntegration;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Transferable.Tournaments;
using Iwentys.Models.Types;

namespace Iwentys.Core.DomainModel
{
    public interface ITournamentDomain
    {
        TournamentLeaderboardDto GetLeaderboard();
    }

    public static class TournamentDomainHelper
    {
        public static ITournamentDomain WrapToDomain(this TournamentEntity tournament, IGithubApiAccessor githubApiAccessor, DatabaseAccessor databaseAccessor, GithubUserDataService githubUserDataService)
        {
            return tournament.Type switch
            {
                TournamentType.CodeMarathon => new CodeMarathonTournament(tournament, githubApiAccessor, databaseAccessor, githubUserDataService),
                _ => throw InnerLogicException.NotSupportedEnumValue(tournament.Type)
            };
        }
    }
}
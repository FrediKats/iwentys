using Iwentys.Core.GithubIntegration;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Context;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Exceptions;
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
        public static ITournamentDomain WrapToDomain(this TournamentEntity tournament, IGithubApiAccessor githubApiAccessor, DatabaseAccessor databaseAccessor, IGithubUserDataService githubUserDataService)
        {
            return tournament.Type switch
            {
                TournamentType.CodeMarathon => new CodeMarathonTournament(tournament, githubApiAccessor, databaseAccessor, githubUserDataService),
                _ => throw InnerLogicException.NotSupportedEnumValue(tournament.Type)
            };
        }
    }
}
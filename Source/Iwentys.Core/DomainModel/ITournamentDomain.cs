using Iwentys.Core.GithubIntegration;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Models.Entities;
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
        public static ITournamentDomain WrapToDomain(this Tournament tournament, IGuildProfileService guildProfileService, IGithubApiAccessor githubApiAccessor)
        {
            return tournament.Type switch
            {
                TournamentType.CodeMarathon => new CodeMarathonTournament(tournament, guildProfileService, githubApiAccessor),
                _ => throw InnerLogicException.NotSupportedEnumValue(tournament.Type)
            };
        }
    }
}
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.Guilds.Enums;
using Iwentys.Domain.Models;
using Iwentys.Domain.Services;

namespace Iwentys.Domain.Guilds
{
    public interface ITournamentDomain
    {
        TournamentLeaderboardDto GetLeaderboard();
        Task RewardWinners();
        Task UpdateResult();
    }

    public static class TournamentDomainHelper
    {
        public static ITournamentDomain WrapToDomain(
            this Tournament tournament,
            GithubIntegrationService githubIntegrationService,
            IUnitOfWork unitOfWork,
            AchievementProvider achievementProvider)
        {
            return tournament.Type switch
            {
                TournamentType.CodeMarathon => new CodeMarathonTournamentDomain(tournament, githubIntegrationService, unitOfWork, achievementProvider),
                _ => throw InnerLogicException.NotSupportedEnumValue(tournament.Type)
            };
        }
    }
}
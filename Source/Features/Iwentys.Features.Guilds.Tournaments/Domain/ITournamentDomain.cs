using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Features.Achievements.Domain;
using Iwentys.Features.GithubIntegration.Services;
using Iwentys.Features.Guilds.Tournaments.Entities;
using Iwentys.Features.Guilds.Tournaments.Enums;
using Iwentys.Features.Guilds.Tournaments.Models;

namespace Iwentys.Features.Guilds.Tournaments.Domain
{
    public interface ITournamentDomain
    {
        TournamentLeaderboardDto GetLeaderboard();
        Task RewardWinners();
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
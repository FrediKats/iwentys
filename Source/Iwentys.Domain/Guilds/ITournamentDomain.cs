namespace Iwentys.Domain.Guilds
{
    public interface ITournamentDomain
    {
        //TournamentLeaderboardDto GetLeaderboard();
        void RewardWinners();
        void UpdateResult();
    }
}
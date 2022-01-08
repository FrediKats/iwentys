using System.Linq;
using Iwentys.Common;
using Iwentys.Domain.GithubIntegration;

namespace Iwentys.Domain.Guilds
{
    public class CodeMarathonTournamentDomain : ITournamentDomain
    {
        private readonly IGithubUserApiAccessor _githubUserApiAccessor;
        private readonly Tournament _tournament;

        public CodeMarathonTournamentDomain(Tournament tournament, IGithubUserApiAccessor githubUserApiAccessor)
        {
            _tournament = tournament;
            _githubUserApiAccessor = githubUserApiAccessor;
        }

        //public TournamentLeaderboardDto GetLeaderboard()
        //{
        //    Dictionary<GuildProfileShortInfoDto, int> result = _tournament
        //        .Teams
        //        .ToDictionary(g => new GuildProfileShortInfoDto(g.Guild), t => CountGuildRating(t.Guild));

        //    return new TournamentLeaderboardDto
        //    {
        //        Tournament = _tournament,
        //        Result = result
        //    };
        //}

        public void RewardWinners()
        {
            if (_tournament.IsActive)
                throw InnerLogicException.TournamentException.IsNotFinished(_tournament.Id);

            TournamentParticipantTeam winner = _tournament
                .Teams
                .OrderByDescending(t => t.Members.Sum(m => m.Points))
                .FirstOrDefault();

            if (winner is null)
                throw InnerLogicException.TournamentException.NoTeamRegistered(_tournament.Id);

            //TODO: fix
            //await _achievementProvider.AchieveForGuild(AchievementList.Tournaments.TournamentWinner, winner.GuildId);
        }

        public void UpdateResult()
        {
            if (!_tournament.IsActive)
                throw InnerLogicException.TournamentException.AlreadyFinished(_tournament.Id);

            foreach (TournamentTeamMember member in _tournament.Teams.SelectMany(t => t.Members))
            {
                //TODO: clean
                ContributionFullInfo contributionFullInfo = _githubUserApiAccessor.FindUserContributionOrEmpty(member.Member).Result;
                member.Points = contributionFullInfo.GetActivityForPeriod(_tournament.StartTime, _tournament.EndTime);
            }
        }

        //private int CountGuildRating(Guild guild)
        //{
        //    //TODO: remove result
        //    List<GuildMemberImpactDto> users = guild.GetMemberImpacts(_githubUserApiAccessor).Result;

        //    return users
        //        .Select(userData => userData.Contribution.GetActivityForPeriod(_tournament.StartTime, _tournament.EndTime))
        //        .Sum();
        //}
    }
}
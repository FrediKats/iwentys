using System;
using Iwentys.Features.GithubIntegration.Services;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.ViewModels.Tournaments;
using Iwentys.Integrations.GithubIntegration;

namespace Iwentys.Features.Guilds.Domain
{
    public class CodeMarathonTournament : ITournamentDomain
    {
        private readonly IGithubApiAccessor _githubApiAccessor;

        private readonly TournamentEntity _tournament;
        private readonly GithubUserDataService _githubUserDataService;

        public CodeMarathonTournament(TournamentEntity tournament, IGithubApiAccessor githubApiAccessor, GithubUserDataService githubUserDataService)
        {
            _tournament = tournament;
            _githubApiAccessor = githubApiAccessor;
            _githubUserDataService = githubUserDataService;
        }

        public TournamentLeaderboardDto GetLeaderboard()
        {
            throw new NotImplementedException();

            //Dictionary<GuildProfileShortInfoDto, int> result = _database
            //    .Guild
            //    .Read()
            //    .ToDictionary(g => new GuildProfileShortInfoDto(g), CountGuildRating);

            //return new TournamentLeaderboardDto
            //{
            //    Tournament = _tournament,
            //    Result = result
            //};
        }

        private int CountGuildRating(GuildEntity guild)
        {
            throw new NotImplementedException();
            //var guildDomain = new GuildDomain(guild, _githubUserDataService, _githubApiAccessor, new GuildRepositoriesScope(_database.Student, _database.Guild, _database.GuildMember, _database.GuildTribute));
            //return guildDomain.GetGithubUserData().Select(userData => userData.ContributionFullInfo.GetActivityForPeriod(_tournament.StartTime, _tournament.EndTime)).Sum();
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Iwentys.Core.DomainModel.Guilds;
using Iwentys.Core.GithubIntegration;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Context;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Transferable.Tournaments;

namespace Iwentys.Core.DomainModel
{
    public class CodeMarathonTournament : ITournamentDomain
    {
        private readonly IGithubApiAccessor _githubApiAccessor;

        private readonly TournamentEntity _tournament;
        private readonly DatabaseAccessor _database;
        private readonly IGithubUserDataService _githubUserDataService;

        public CodeMarathonTournament(TournamentEntity tournament, IGithubApiAccessor githubApiAccessor, DatabaseAccessor database, IGithubUserDataService githubUserDataService)
        {
            _tournament = tournament;
            _githubApiAccessor = githubApiAccessor;
            _database = database;
            _githubUserDataService = githubUserDataService;
        }

        public TournamentLeaderboardDto GetLeaderboard()
        {
            Dictionary<GuildProfileShortInfoDto, int> result = _database
                .GuildRepository
                .Read()
                .ToDictionary(g => new GuildProfileShortInfoDto(g), CountGuildRating);

            return new TournamentLeaderboardDto
            {
                Tournament = _tournament,
                Result = result
            };
        }

        private int CountGuildRating(GuildEntity guild)
        {
            var guildDomain = new GuildDomain(guild, _database, _githubUserDataService, _githubApiAccessor);
            return guildDomain.GetGithubUserData().Select(userData => userData.ContributionFullInfo.GetActivityForPeriod(_tournament.StartTime, _tournament.EndTime)).Sum();
        }
    }
}
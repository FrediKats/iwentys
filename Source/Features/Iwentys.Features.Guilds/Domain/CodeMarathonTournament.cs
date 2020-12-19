using System;
using Iwentys.Features.GithubIntegration;
using Iwentys.Features.GithubIntegration.Services;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Models.Tournaments;
//TODO:
// ReSharper disable NotAccessedField.Local

namespace Iwentys.Features.Guilds.Domain
{
    public class CodeMarathonTournament : ITournamentDomain
    {
        private readonly IGithubApiAccessor _githubApiAccessor;
        private readonly TournamentEntity _tournament;
        private readonly GithubIntegrationService _githubIntegrationService;

        public CodeMarathonTournament(TournamentEntity tournament, IGithubApiAccessor githubApiAccessor, GithubIntegrationService githubIntegrationService)
        {
            _tournament = tournament;
            _githubApiAccessor = githubApiAccessor;
            _githubIntegrationService = githubIntegrationService;
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

        //TODO:
        //private int CountGuildRating(GuildEntity guild)
        //{
        //    throw new NotImplementedException();
        //    //var guildDomain = new GuildDomain(guild, _githubIntegrationService, _githubApiAccessor, new GuildRepositoriesScope(_database.Student, _database.Guild, _database.GuildMember, _database.GuildTribute));
        //    //return guildDomain.GetGithubUserData().Select(userData => userData.ContributionFullInfo.GetActivityForPeriod(_tournament.StartTime, _tournament.EndTime)).Sum();
        //}
    }
}
using System.Collections.Generic;
using System.Linq;
using Iwentys.Core.GithubIntegration;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Models.Entities;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Transferable.Tournaments;

namespace Iwentys.Core.DomainModel
{
    public class CodeMarathonTournament : ITournamentDomain
    {
        private readonly IGithubApiAccessor _githubApiAccessor;

        private readonly IGuildService _guildService;
        private readonly Tournament _tournament;

        public CodeMarathonTournament(Tournament tournament, IGuildService guildService, IGithubApiAccessor githubApiAccessor)
        {
            _tournament = tournament;
            _guildService = guildService;
            _githubApiAccessor = githubApiAccessor;
        }

        public TournamentLeaderboardDto GetLeaderboard()
        {
            Dictionary<GuildProfileDto, int> result = _guildService
                .Get()
                .ToDictionary(c => c, CalculateGuildPoints);

            return new TournamentLeaderboardDto
            {
                Tournament = _tournament,
                Result = result
            };
        }

        private int CalculateGuildPoints(GuildProfileDto guildProfile)
        {
            return guildProfile
                .Members
                .Where(m => m.GithubUsername != null)
                .Sum(m => _githubApiAccessor.GetUserActivity(m.GithubUsername, _tournament.StartTime, _tournament.EndTime));
        }
    }
}
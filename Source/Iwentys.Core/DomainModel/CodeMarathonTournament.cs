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
        private readonly Tournament _tournament;

        private readonly IGuildProfileService _guildProfileService;
        private readonly IGithubApiAccessor _githubApiAccessor;

        public CodeMarathonTournament(Tournament tournament, IGuildProfileService guildProfileService, IGithubApiAccessor githubApiAccessor)
        {
            _tournament = tournament;
            _guildProfileService = guildProfileService;
            _githubApiAccessor = githubApiAccessor;
        }

        public TournamentLeaderboardDto GetLeaderboard()
        {
            Dictionary<GuildProfileDto, int> result = _guildProfileService
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
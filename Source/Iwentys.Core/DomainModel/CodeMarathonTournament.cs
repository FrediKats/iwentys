using System.Collections.Generic;
using System.Linq;
using Iwentys.Core.GithubIntegration;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Transferable.Tournaments;

namespace Iwentys.Core.DomainModel
{
    public class CodeMarathonTournament : ITournamentDomain
    {
        private readonly IGithubApiAccessor _githubApiAccessor;

        private readonly IGuildService _guildService;
        private readonly TournamentEntity _tournament;

        public CodeMarathonTournament(TournamentEntity tournament, IGuildService guildService, IGithubApiAccessor githubApiAccessor)
        {
            _tournament = tournament;
            _guildService = guildService;
            _githubApiAccessor = githubApiAccessor;
        }

        public TournamentLeaderboardDto GetLeaderboard()
        {
            Dictionary<GuildProfileDto, int> result = _guildService
                .Get()
                .ToDictionary(c => c, g => g.MemberLeaderBoard.TotalRate);

            return new TournamentLeaderboardDto
            {
                Tournament = _tournament,
                Result = result
            };
        }
    }
}
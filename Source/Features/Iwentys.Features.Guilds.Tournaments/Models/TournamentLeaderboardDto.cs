using System.Collections.Generic;
using Iwentys.Features.Guilds.Models;
using Iwentys.Features.Guilds.Tournaments.Entities;

namespace Iwentys.Features.Guilds.Tournaments.Models
{
    public class TournamentLeaderboardDto
    {
        public TournamentEntity Tournament { get; set; }
        public Dictionary<GuildProfileShortInfoDto, int> Result { get; set; }
    }
}
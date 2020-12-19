using System.Collections.Generic;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Models.Guilds;

namespace Iwentys.Features.Guilds.Models.Tournaments
{
    public class TournamentLeaderboardDto
    {
        public TournamentEntity Tournament { get; set; }
        public Dictionary<GuildProfileShortInfoDto, int> Result { get; set; }
    }
}
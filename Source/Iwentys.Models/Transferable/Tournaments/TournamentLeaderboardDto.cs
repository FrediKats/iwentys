using System.Collections.Generic;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Transferable.Guilds;

namespace Iwentys.Models.Transferable.Tournaments
{
    public class TournamentLeaderboardDto
    {
        public Tournament Tournament { get; set; }
        public Dictionary<GuildProfileDto, int> Result { get; set; }
    }
}
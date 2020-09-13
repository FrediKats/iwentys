using System.Collections.Generic;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Transferable.Guilds;

namespace Iwentys.Models.Transferable.Tournaments
{
    public class TournamentLeaderboardDto
    {
        public TournamentEntity Tournament { get; set; }
        public Dictionary<GuildProfileShortInfoDto, int> Result { get; set; }
    }
}
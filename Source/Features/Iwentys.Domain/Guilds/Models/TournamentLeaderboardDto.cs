using System.Collections.Generic;

namespace Iwentys.Domain.Guilds.Models
{
    public class TournamentLeaderboardDto
    {
        public Tournament Tournament { get; set; }
        public Dictionary<GuildProfileShortInfoDto, int> Result { get; set; }
    }
}
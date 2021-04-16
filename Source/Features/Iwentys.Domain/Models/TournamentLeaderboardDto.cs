using System.Collections.Generic;
using Iwentys.Domain.Guilds;

namespace Iwentys.Domain.Models
{
    public class TournamentLeaderboardDto
    {
        public Tournament Tournament { get; set; }
        public Dictionary<GuildProfileShortInfoDto, int> Result { get; set; }
    }
}
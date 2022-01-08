using System.Collections.Generic;
using Iwentys.Domain.Guilds;

namespace Iwentys.Guilds
{
    public class TournamentLeaderboardDto
    {
        public Tournament Tournament { get; set; }
        public Dictionary<GuildProfileShortInfoDto, int> Result { get; set; }
    }
}
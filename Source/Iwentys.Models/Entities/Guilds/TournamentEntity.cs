using System;
using Iwentys.Models.Types.Guilds;

namespace Iwentys.Models.Entities.Guilds
{
    public class TournamentEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TournamentType Type { get; set; }
    }
}
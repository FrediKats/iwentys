using System;
using Iwentys.Features.Guilds.Tournaments.Enums;

namespace Iwentys.Features.Guilds.Tournaments.Entities
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
using System;

namespace Iwentys.Features.Guilds.Tournaments.Models
{
    public class CreateTournamentArguments
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
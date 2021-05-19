using System;

namespace Iwentys.Domain.Raids.Models
{
    public class RaidCreateArguments
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTimeUtc { get; set; }
        public DateTime EndTimeUtc { get; set; }
    }
}
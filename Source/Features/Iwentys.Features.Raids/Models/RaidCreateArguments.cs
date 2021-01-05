using System;

namespace Iwentys.Features.Raids.Models
{
    public class RaidCreateArguments
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTimeUtc { get; set; }
        public DateTime EndTimeUtc { get; set; }
    }
}
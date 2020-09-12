using System;

namespace Iwentys.Models.Entities.Guilds
{
    public class GuildTestTaskSolvingInfoEntity
    {
        public int GuildId { get; set; }
        public Guild Guild { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime? SubmitTime { get; set; }
        public DateTime? CompleteTime { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Types.Guilds;

namespace Iwentys.Models.Entities.Guilds
{
    public class Tribute
    {
        public Guild Guild { get; set; }
        public int GuildId { get; set; }

        public StudentProject Project { get; set; }
        [Key]
        public int ProjectId { get; set; }

        public TributeState State { get; set; }
        public int DifficultLevel { get; set; }
        public int Mark { get; set; }
        public DateTime CreationTime { get; set; }

        public Student Mentor { get; set; }
        public int MentorId { get; set; }

        public static Tribute New(int guildId, int projectId)
        {
            return new Tribute
            {
                GuildId = guildId,
                ProjectId = projectId,
                State = TributeState.Pending,
                CreationTime = DateTime.UtcNow
            };
        }

        public void SetCanceled()
        {
            State = TributeState.Canceled;
        }

        public void SetCompleted(int mentorId, int difficultLevel, int mark)
        {
            if (State == TributeState.Pending)
                throw new InnerLogicException($"Can't completed tribute. It's in state [{State}]");

            MentorId = mentorId;
            DifficultLevel = difficultLevel;
            Mark = mark;
            State = TributeState.Completed;
        }
    }
}
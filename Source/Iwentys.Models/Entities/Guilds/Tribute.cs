using System;
using System.ComponentModel.DataAnnotations;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Types.Guilds;

namespace Iwentys.Models.Entities.Guilds
{
    public class Tribute
    {
        [Key]
        public long ProjectId { get; set; }
        public GithubProjectEntity ProjectEntity { get; set; }

        public GuildEntity Guild { get; set; }
        public int GuildId { get; set; }

        public TributeState State { get; set; }
        public int? DifficultLevel { get; set; }
        public int? Mark { get; set; }
        public DateTime CreationTime { get; set; }

        public Student Mentor { get; set; }
        public int? MentorId { get; set; }

        public Tribute()
        {
        }

        public Tribute(GuildEntity guild, GithubProjectEntity projectEntity) : this()
        {
            GuildId = guild.Id;
            ProjectEntity = projectEntity;
            ProjectId = projectEntity.Id;
            State = TributeState.Active;
            CreationTime = DateTime.UtcNow;
        }

        public void SetCanceled()
        {
            State = TributeState.Canceled;
        }

        public void SetCompleted(int mentorId, int difficultLevel, int mark)
        {
            if (State != TributeState.Active)
                throw InnerLogicException.TributeEx.IsNotActive(this);

            MentorId = mentorId;
            DifficultLevel = difficultLevel;
            Mark = mark;
            State = TributeState.Completed;
        }
    }
}
using System;
using System.Linq.Expressions;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Guilds.Entities
{
    public class GuildTestTaskSolution
    {
        public GuildTestTaskSolution()
        {
        }

        public GuildTestTaskSolution(int guildId, int studentId) : this()
        {
            GuildId = guildId;
            StudentId = studentId;
            StartTime = DateTime.UtcNow;
        }

        public int GuildId { get; init; }
        public virtual Guild Guild { get; init; }
        
        public int StudentId { get; init; }
        public virtual Student Student { get; init; }

        public long? ProjectId { get; set; }
        public virtual GithubProject Project { get; set; }

        public int? ReviewerId { get; set; }
        public virtual Student Reviewer { get; set; }
        
        public DateTime StartTime { get; init; }
        public DateTime? SubmitTime { get; set; }
        public DateTime? CompleteTime { get; set; }

        public static GuildTestTaskSolution Create(Guild guild, Student student)
        {
            return new GuildTestTaskSolution
            {
                GuildId = guild.Id,
                StudentId = student.Id,
                StartTime = DateTime.UtcNow
            };
        }

        public static Expression<Func<GuildTestTaskSolution, bool>> IsNotCompleted => entity => entity.CompleteTime != null;

        public void SendSubmit(long projectId)
        {
            ProjectId = projectId;
            SubmitTime = DateTime.UtcNow;
        }

        public void SetCompleted(Student reviewer)
        {
            ReviewerId = reviewer.Id;
            CompleteTime = DateTime.UtcNow;
        }

        public GuildTestTaskState GetState()
        {
            if (CompleteTime is not null)
                return GuildTestTaskState.Completed;

            if (SubmitTime is not null)
                return GuildTestTaskState.Submitted;

            return GuildTestTaskState.Started;
        }
    }
}
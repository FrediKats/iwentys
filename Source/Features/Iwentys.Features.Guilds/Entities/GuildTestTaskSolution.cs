using System;
using System.Linq.Expressions;
using Iwentys.Features.AccountManagement.Entities;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.Guilds.Enums;

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
        public virtual IwentysUser Student { get; init; }

        public long? ProjectId { get; set; }
        public virtual GithubProject Project { get; set; }

        public int? ReviewerId { get; set; }
        public virtual IwentysUser Reviewer { get; set; }
        
        public DateTime StartTime { get; init; }
        public DateTime? SubmitTime { get; set; }
        public DateTime? CompleteTime { get; set; }

        public static GuildTestTaskSolution Create(Guild guild, IwentysUser student)
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

        public void SetCompleted(IwentysUser reviewer)
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
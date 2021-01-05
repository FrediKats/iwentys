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

        public GuildTestTaskSolution(int guildId, int authorId) : this()
        {
            GuildId = guildId;
            AuthorId = authorId;
            StartTimeUtc = DateTime.UtcNow;
        }

        public int GuildId { get; init; }
        public virtual Guild Guild { get; init; }
        
        public int AuthorId { get; init; }
        public virtual IwentysUser Author { get; init; }

        public long? ProjectId { get; set; }
        public virtual GithubProject Project { get; set; }

        public int? ReviewerId { get; set; }
        public virtual IwentysUser Reviewer { get; set; }
        
        public DateTime StartTimeUtc { get; init; }
        public DateTime? SubmitTimeUtc { get; set; }
        public DateTime? CompleteTimeUtc { get; set; }

        public static GuildTestTaskSolution Create(Guild guild, IwentysUser author)
        {
            return new GuildTestTaskSolution
            {
                GuildId = guild.Id,
                AuthorId = author.Id,
                StartTimeUtc = DateTime.UtcNow
            };
        }

        public static Expression<Func<GuildTestTaskSolution, bool>> IsNotCompleted => entity => entity.CompleteTimeUtc != null;

        public void SendSubmit(long projectId)
        {
            ProjectId = projectId;
            SubmitTimeUtc = DateTime.UtcNow;
        }

        public void SetCompleted(IwentysUser reviewer)
        {
            ReviewerId = reviewer.Id;
            CompleteTimeUtc = DateTime.UtcNow;
        }

        public GuildTestTaskState GetState()
        {
            if (CompleteTimeUtc is not null)
                return GuildTestTaskState.Completed;

            if (SubmitTimeUtc is not null)
                return GuildTestTaskState.Submitted;

            return GuildTestTaskState.Started;
        }
    }
}
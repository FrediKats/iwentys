using System;
using System.Linq;
using System.Linq.Expressions;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds.Enums;
using Iwentys.Domain.PeerReview;

namespace Iwentys.Domain.Guilds
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


        public int? ProjectReviewRequestId { get; set; }
        public virtual ProjectReviewRequest ProjectReviewRequest { get; set; }

        public int? ReviewerId { get; set; }
        public virtual IwentysUser Reviewer { get; set; }

        public DateTime StartTimeUtc { get; init; }
        public DateTime? SubmitTimeUtc { get; set; }
        public DateTime? CompleteTimeUtc { get; set; }

        public static Expression<Func<GuildTestTaskSolution, bool>> IsNotCompleted => entity => entity.CompleteTimeUtc != null;

        public static GuildTestTaskSolution Create(Guild guild, IwentysUser author)
        {
            GuildTestTaskSolution existedTestTaskSolution = guild
                .TestTasks
                .Where(GuildTestTaskSolution.IsNotCompleted.Compile())
                .FirstOrDefault(k => k.AuthorId == author.Id);

            if (existedTestTaskSolution is not null)
                InnerLogicException.GuildExceptions.ActiveTestExisted(author.Id, guild.Id);

            return new GuildTestTaskSolution
            {
                GuildId = guild.Id,
                AuthorId = author.Id,
                StartTimeUtc = DateTime.UtcNow
            };
        }

        public void SendSubmit(IwentysUser author, ProjectReviewRequest reviewRequest)
        {
            ProjectReviewRequestId = reviewRequest.Id;
            SubmitTimeUtc = DateTime.UtcNow;
        }

        public void SetCompleted(IwentysUser reviewer)
        {
            Guild.EnsureIsGuildMentor(reviewer);

            if (GetState() != GuildTestTaskState.Submitted)
                throw new InnerLogicException("Task must be submitted");

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
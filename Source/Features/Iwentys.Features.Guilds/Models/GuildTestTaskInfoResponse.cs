using System;
using System.Linq.Expressions;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;

namespace Iwentys.Features.Guilds.Models
{
    public class GuildTestTaskInfoResponse
    {
        public int StudentId { get; set; }
        public DateTime StartTimeUtc { get; set; }
        public DateTime? SubmitTimeUtc { get; set; }
        public long? ProjectId { get; set; }
        public DateTime? CompleteTimeUtc { get; set; }

        public GuildTestTaskState TestTaskState { get; set; }

        public static Expression<Func<GuildTestTaskSolution, GuildTestTaskInfoResponse>> FromEntity =>
            testTask =>
                new GuildTestTaskInfoResponse
                {
                    StudentId = testTask.AuthorId,
                    StartTimeUtc = testTask.StartTimeUtc,
                    SubmitTimeUtc = testTask.SubmitTimeUtc,
                    ProjectId = testTask.ProjectId,
                    CompleteTimeUtc = testTask.CompleteTimeUtc,
                    TestTaskState = testTask.GetState()
                };


        public static GuildTestTaskInfoResponse Wrap(GuildTestTaskSolution testTask)
        {
            return new GuildTestTaskInfoResponse
            {
                StudentId = testTask.AuthorId,
                StartTimeUtc = testTask.StartTimeUtc,
                SubmitTimeUtc = testTask.SubmitTimeUtc,
                ProjectId = testTask.ProjectId,
                CompleteTimeUtc = testTask.CompleteTimeUtc,
                TestTaskState = testTask.GetState()
            };
        }
    }
}
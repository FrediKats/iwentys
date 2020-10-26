using System;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Types;

namespace Iwentys.Models.Transferable.Guilds
{
    public class GuildTestTaskInfoResponse
    {
        public int StudentId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? SubmitTime { get; set; }
        public long? ProjectId { get; set; }
        public DateTime? CompleteTime { get; set; }

        public GuildTestTaskState TestTaskState { get; set; }

        public static GuildTestTaskInfoResponse Wrap(GuildTestTaskSolvingInfoEntity testTask)
        {
            return new GuildTestTaskInfoResponse
            {
                StudentId = testTask.StudentId,
                StartTime = testTask.StartTime,
                SubmitTime = testTask.SubmitTime,
                ProjectId = testTask.ProjectId,
                CompleteTime = testTask.CompleteTime,
                TestTaskState = testTask.GetState()
            };
        }
    }
}
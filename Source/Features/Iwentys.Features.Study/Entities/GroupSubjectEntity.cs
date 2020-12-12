using System;
using System.Text.Json;
using FluentResults;
using Iwentys.Features.Students.Enums;

namespace Iwentys.Features.Study.Entities
{
    public class GroupSubjectEntity
    {
        public int Id { get; set; }

        public int SubjectId { get; set; }
        public virtual SubjectEntity Subject { get; set; }

        public int StudyGroupId { get; set; }
        public virtual StudyGroupEntity StudyGroup { get; set; }

        public int LectorTeacherId { get; set; }
        public virtual TeacherEntity LectorTeacher { get; set; }

        public int PracticeTeacherId { get; set; }
        public virtual TeacherEntity PracticeTeacher { get; set; }

        public string SerializedGoogleTableConfig { get; set; }
        public StudySemester StudySemester { get; set; }

        public Result<GoogleTableData> TryGetGoogleTableDataConfig()
        {
            if (SerializedGoogleTableConfig is null)
                return Result.Fail<GoogleTableData>("Value is not set");

            try
            {
                var googleTableData = JsonSerializer.Deserialize<GoogleTableData>(SerializedGoogleTableConfig);
                return Result.Ok(googleTableData);
            }
            catch (Exception e)
            {
                return Result.Fail<GoogleTableData>(new Error("Data parse failed").CausedBy(e));
            }
        }
    }
}
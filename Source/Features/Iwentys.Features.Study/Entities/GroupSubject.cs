using System;
using System.Linq;
using System.Text.Json;
using FluentResults;
using Iwentys.Common.Databases;
using Iwentys.Features.Students.Enums;
using Iwentys.Features.Study.Models;

namespace Iwentys.Features.Study.Entities
{
    public class GroupSubject
    {
        public int Id { get; set; }

        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }

        public int StudyGroupId { get; set; }
        public virtual StudyGroup StudyGroup { get; set; }

        public int LectorTeacherId { get; set; }
        public virtual Teacher LectorTeacher { get; set; }

        public int PracticeTeacherId { get; set; }
        public virtual Teacher PracticeTeacher { get; set; }

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
    
    public static class GroupSubjectExtensions
    {
        public static IQueryable<Subject> SearchSubjects(this IQueryable<GroupSubject> query, StudySearchParametersDto searchParametersDto)
        {
            IQueryable<Subject> newQuery = query
                .WhereIf(searchParametersDto.GroupId, gs => gs.StudyGroupId == searchParametersDto.GroupId)
                .WhereIf(searchParametersDto.StudySemester, gs => gs.StudySemester == searchParametersDto.StudySemester)
                .WhereIf(searchParametersDto.SubjectId, gs => gs.SubjectId == searchParametersDto.SubjectId)
                .WhereIf(searchParametersDto.CourseId, gs => gs.StudyGroup.StudyCourseId == searchParametersDto.CourseId)
                .Select(s => s.Subject)
                .Distinct();

            return newQuery;
        }
    }
}
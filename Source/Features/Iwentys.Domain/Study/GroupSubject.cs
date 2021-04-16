using System;
using System.Linq;
using System.Text.Json;
using FluentResults;
using Iwentys.Common.Databases;
using Iwentys.Domain.Models;
using Iwentys.Domain.Study.Enums;

namespace Iwentys.Domain.Study
{
    public class GroupSubject
    {
        public int Id { get; init; }

        public int SubjectId { get; init; }
        public virtual Subject Subject { get; init; }
        public StudySemester StudySemester { get; init; }

        public int StudyGroupId { get; init; }
        public virtual StudyGroup StudyGroup { get; init; }

        public int? LectorTeacherId { get; init; }
        public virtual UniversitySystemUser LectorTeacher { get; init; }

        public int? PracticeTeacherId { get; init; }
        public virtual UniversitySystemUser PracticeTeacher { get; init; }


        public string SerializedGoogleTableConfig { get; set; }

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
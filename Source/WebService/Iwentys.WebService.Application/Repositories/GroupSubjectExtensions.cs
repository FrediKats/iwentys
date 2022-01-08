using System.Linq;
using Iwentys.DataAccess;
using Iwentys.Domain.Study;

namespace Iwentys.WebService.Application;

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
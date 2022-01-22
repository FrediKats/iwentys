using System.Collections.Generic;
using System.Linq;
using Iwentys.DataAccess;
using Iwentys.Domain.Study;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.WebService.Application;

public static class StudyDbContextExtensions
{
    //TODO: make async
    public static IReadOnlyCollection<SubjectActivity> GetStudentActivities(this IwentysDbContext dbContext, StudySearchParametersDto searchParametersDto)
    {
        var query =
            from sa in dbContext.SubjectActivities
            join gs in dbContext.GroupSubjects on sa.SubjectId equals gs.SubjectId
            select new { SubjectActivities = sa, GroupSubjects = gs };

        query
            .Include(r => r.GroupSubjects)
            .ThenInclude(gs => gs.Subject);

        query = query
            .WhereIf(searchParametersDto.SubjectId, q => q.GroupSubjects.SubjectId == searchParametersDto.SubjectId)
            .WhereIf(searchParametersDto.StudySemester, q => q.GroupSubjects.StudySemester == searchParametersDto.StudySemester);

        return query
            .Select(_ => _.SubjectActivities)
            .ToList();
    }
}
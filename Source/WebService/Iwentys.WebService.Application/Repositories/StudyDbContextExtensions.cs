using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.Study;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.WebService.Application;

public static class StudyDbContextExtensions
{
    //TODO: make async
    public static async Task<IReadOnlyCollection<SubjectActivity>> GetStudentActivities(this IwentysDbContext dbContext, StudySearchParametersDto searchParametersDto)
    {
        List<SubjectActivity> subjectActivities = await dbContext
            .SubjectActivities
            .Include(activity => activity.StudentPosition)
            .WhereIf(searchParametersDto.SubjectId, activity => activity.SubjectId == searchParametersDto.SubjectId)
            .WhereIf(searchParametersDto.GroupId, activity => activity.StudentPosition.GroupId == searchParametersDto.GroupId)
            .WhereIf(searchParametersDto.CourseId, activity => activity.StudentPosition.CourseId == searchParametersDto.CourseId)
            .ToListAsync();

        return subjectActivities;
    }
}
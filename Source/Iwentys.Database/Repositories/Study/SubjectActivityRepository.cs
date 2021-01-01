using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Databases;
using Iwentys.Database.Context;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Models;
using Iwentys.Features.Study.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Database.Repositories.Study
{
    public class SubjectActivityRepository : ISubjectActivityRepository
    {
        private readonly IwentysDbContext _dbContext;

        public SubjectActivityRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IReadOnlyCollection<SubjectActivity> GetStudentActivities(StudySearchParametersDto searchParametersDto)
        {
            var query =
                from sa in _dbContext.SubjectActivities
                join sg in _dbContext.StudyGroups on sa.Student.GroupId equals sg.Id
                join gs in _dbContext.GroupSubjects on sa.GroupSubjectId equals gs.Id
                select new { SubjectActivities = sa, StudyGroups = sg, GroupSubjects = gs };

            query.Include(r => r.StudyGroups)
                .Include(r => r.GroupSubjects)
                .ThenInclude(gs => gs.Subject);

            query = query
                .WhereIf(searchParametersDto.GroupId, q => q.StudyGroups.Id == searchParametersDto.GroupId)
                .WhereIf(searchParametersDto.SubjectId, q => q.GroupSubjects.SubjectId == searchParametersDto.SubjectId)
                .WhereIf(searchParametersDto.CourseId, q => q.StudyGroups.StudyCourseId == searchParametersDto.CourseId)
                .WhereIf(searchParametersDto.StudySemester, q => q.GroupSubjects.StudySemester == searchParametersDto.StudySemester);

            return query
                .Select(_ => _.SubjectActivities)
                .ToList();
        }
    }
}
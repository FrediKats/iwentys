using System.Collections.Generic;
using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Database.Tools;
using Iwentys.Models;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Transferable.Study;
using MoreLinq;

namespace Iwentys.Core.Services
{
    public class StudyLeaderboardService
    {
        private readonly DatabaseAccessor _databaseAccessor;
        private readonly GithubUserDataService _githubUserDataService;

        public StudyLeaderboardService(DatabaseAccessor databaseAccessor, GithubUserDataService githubUserDataService)
        {
            _databaseAccessor = databaseAccessor;
            _githubUserDataService = githubUserDataService;
        }

        public List<SubjectEntity> GetSubjectsForDto(StudySearchParameters searchParameters)
        {
            return _databaseAccessor.GroupSubject.GetSubjectsForDto(searchParameters).DistinctBy(s => s.Id).ToList();
        }

        public List<StudyGroupEntity> GetStudyGroupsForDto(int? courseId)
        {
            return _databaseAccessor.GroupSubject.GetStudyGroupsForDto(courseId).ToList();
        }

        public List<StudyLeaderboardRow> GetStudentsRatings(StudySearchParameters searchParameters)
        {
            if (searchParameters.CourseId == null && searchParameters.GroupId == null ||
                searchParameters.CourseId != null && searchParameters.GroupId != null)
            {
                throw new IwentysException("One of StudySearchParameters fields: CourseId or GroupId should be null");
            }

            List<SubjectActivityEntity> result = _databaseAccessor.SubjectActivity.GetStudentActivities(searchParameters).ToList();

            return result
                .GroupBy(r => r.StudentId)
                .Select(g => new StudyLeaderboardRow(g))
                .OrderByDescending(a => a.Activity)
                .ToList();
        }

        public List<StudyLeaderboardRow> GetCodingRating(int? courseId)
        {
            IQueryable<StudentEntity> query = _databaseAccessor.Student.Read();

            query = query
                .WhereIf(courseId, () => query.Where(q => q.Group.StudyCourseId == courseId));

            return query.AsEnumerable()
                .Select(s => new StudyLeaderboardRow(s, _githubUserDataService.FindByUsername(s.GithubUsername)?.ContributionFullInfo.Total ?? 0))
                .OrderBy(a => a.Activity)
                .ToList();
        }
    }
}

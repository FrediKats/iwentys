using System.Collections.Generic;
using System.Linq;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Context;
using Iwentys.Models;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Transferable.Study;
using Iwentys.Models.Types;
using MoreLinq;

namespace Iwentys.Core.Services.Implementations
{
    public class StudyLeaderboardService : IStudyLeaderboardService
    {
        private readonly DatabaseAccessor _databaseAccessor;

        public StudyLeaderboardService(DatabaseAccessor databaseAccessor)
        {
            _databaseAccessor = databaseAccessor;
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

            //TODO: allow null value
            searchParameters.StudySemester ??= GetCurrentSemester();

            List<SubjectActivityEntity> result = _databaseAccessor.SubjectActivity.GetStudentActivities(searchParameters).ToList();

            return result
                .GroupBy(r => r.StudentId)
                .Select(g => new StudyLeaderboardRow(g))
                .OrderByDescending(a => a.Activity)
                .ToList();
        }

        private static StudySemester GetCurrentSemester()
        {
            //TODO: hack
            return StudySemester.Y20H2;
        }
    }
}

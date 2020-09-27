using System.Collections.Generic;
using System.Linq;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Context;
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

        public List<SubjectEntity> GetSubjectsForDto(StudySearchDto searchDto)
        {
            return _databaseAccessor.GroupSubject.GetSubjectsForDto(searchDto).DistinctBy(s => s.Id).ToList();
        }

        public List<StudyGroupEntity> GetStudyGroupsForDto(int? courseId)
        {
            return _databaseAccessor.GroupSubject.GetStudyGroupsForDto(courseId).ToList();
        }

        public List<StudyLeaderboardRow> GetStudentsRatings(StudySearchDto searchDto)
        {
            if (searchDto.CourseId == null && searchDto.GroupId == null ||
                searchDto.CourseId != null && searchDto.GroupId != null)
            {
                throw new IwentysException("One of StudySearchDto fields: CourseId or GroupId should be null");
            }

            //TODO: allow null value
            searchDto.StudySemester ??= GetCurrentSemester();

            List<SubjectActivityEntity> result = _databaseAccessor.SubjectActivity.GetStudentActivities(searchDto).ToList();

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

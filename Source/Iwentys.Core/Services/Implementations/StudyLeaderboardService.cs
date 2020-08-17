using System;
using System.Collections.Generic;
using System.Linq;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Transferable.Study;
using Iwentys.Models.Types;

namespace Iwentys.Core.Services.Implementations
{
    public class StudyLeaderboardService : IStudyLeaderboardService
    {
        private ISubjectForGroupRepository _subjectForGroupRepository;
        private ISubjectActivityRepository _subjectActivityRepository;
        public StudyLeaderboardService(ISubjectForGroupRepository subjectForGroupRepository, ISubjectActivityRepository subjectActivityRepository)
        {
            _subjectForGroupRepository = subjectForGroupRepository;
            _subjectActivityRepository = subjectActivityRepository;
        }

        public IEnumerable<Subject> GetSubjectsForDto(StudySearchDto searchDto)
        {
            return _subjectForGroupRepository.GetSubjectsForDto(searchDto);
        }

        public IEnumerable<StudyGroup> GetStudyGroupsForDto(StudySearchDto searchDto)
        {
            return _subjectForGroupRepository.GetStudyGroupsForDto(searchDto);
        }

        public List<StudyLeaderboardRow> GetStudentsRatings(StudySearchDto searchDto)
        {
            if (searchDto.StreamId == null && searchDto.GroupId == null ||
                searchDto.StreamId != null && searchDto.GroupId != null)
            {
                throw new IwentysException("One of StudySearchDto fields: StreamId or GroupId should be null");
            }

            searchDto.StudySemester ??= GetCurrentSemester();

            List<SubjectActivity> result = _subjectActivityRepository.GetStudentActivities(searchDto).ToList();

            return result
                .GroupBy(r => r.StudentId)
                .Select(g => new StudyLeaderboardRow(g))
                .OrderByDescending(a => a.Activity)
                .ToList();
        }

        private static StudySemester GetCurrentSemester()
        {
            throw new NotImplementedException();
        }
    }
}

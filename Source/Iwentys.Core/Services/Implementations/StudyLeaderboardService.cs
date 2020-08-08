using System;
using System.Collections.Generic;
using System.Linq;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Transferable.Study;
using Iwentys.Models.Types;

namespace Iwentys.Core.Services.Implementations
{
    public class StudyLeaderboardService : IStudyLeaderboardService
    {
        private IStudyImmutableDataRepository _studyImmutableDataRepository;
        private ISubjectForGroupRepository _subjectForGroupRepository;
        private ISubjectActivityRepository _subjectActivityRepository;
        public StudyLeaderboardService(ISubjectForGroupRepository subjectForGroupRepository, ISubjectActivityRepository subjectActivityRepository, IStudyImmutableDataRepository studyImmutableDataRepository)
        {
            _subjectForGroupRepository = subjectForGroupRepository;
            _subjectActivityRepository = subjectActivityRepository;
            _studyImmutableDataRepository = studyImmutableDataRepository;
        }

        public IEnumerable<Subject> GetSubjectsForDto(StudySearchDto searchDto)
        {
            return _subjectForGroupRepository.GetSubjectsForDto(searchDto);
        }

        public IEnumerable<StudyGroup> GetStudyGroupsForDto(StudySearchDto searchDto)
        {
            return _subjectForGroupRepository.GetStudyGroupsForDto(searchDto);
        }

        public IEnumerable<SubjectActivity> GetStudentsRatings(StudySearchDto searchDto)
        {
            if (searchDto.StreamId == null && searchDto.GroupId == null ||
                searchDto.StreamId != null && searchDto.GroupId != null)
            {
                throw new Exception();
            }

            searchDto.StudySemester ??= GetCurrentSemester();

            List<SubjectActivity> result;
            if (searchDto.SubjectId != null)
                result = GetStudentsRatingsForSubject(searchDto);
            else
                result = GetStudentsRatingsForAllSubjects(searchDto);

            return result.OrderByDescending(s => s.Points);
        }

        private List<SubjectActivity> GetStudentsRatingsForSubject(StudySearchDto searchDto)
        {
            var result = new List<SubjectActivity>();

            if (searchDto.GroupId != null)
            {
                var subjectForGroup = _subjectForGroupRepository.GetSubjectForGroupForDto(searchDto).SingleOrDefault();
                result = GetStudentsFromGroupRatingsForSubject(subjectForGroup);
            }
            else
            {
                foreach (var subjectForGroup in _subjectForGroupRepository.GetSubjectForGroupForDto(searchDto))
                {
                    result.AddRange(GetStudentsFromGroupRatingsForSubject(subjectForGroup));
                }
            }

            return result;
        }

        private List<SubjectActivity> GetStudentsFromGroupRatingsForSubject(SubjectForGroup subjectForGroup)
        {
            var result = new List<SubjectActivity>();
            if (subjectForGroup == null)
            {
                //TODO: Logging
                return result;
            }

            var students = _studyImmutableDataRepository.GetStudentsForGroup(subjectForGroup.StudyGroup.NamePattern);
            foreach (var student in students)
            {
                var subjectForGroupActivity =
                    _subjectActivityRepository.GetActivityForStudentAndSubject(student.Id,
                        subjectForGroup.Id);
                if (subjectForGroupActivity == null)
                {
                    //TODO: Logging
                }
                else
                {
                    result.Add(subjectForGroupActivity);
                }
            }

            return result;
        }

        private List<SubjectActivity> GetStudentsRatingsForAllSubjects(StudySearchDto searchDto)
        {
            var result = new List<SubjectActivity>();

            foreach (var subject in _subjectForGroupRepository.GetSubjectsForDto(searchDto))
            {
                searchDto.SubjectId = subject.Id;
                result.AddRange(GetStudentsRatingsForSubject(searchDto));
            }

            return result;
        }

        private static StudySemester GetCurrentSemester()
        {
            throw new NotImplementedException();
        }
    }
}

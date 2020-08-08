using System;
using System.Collections.Generic;
using System.Linq;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Types;
using LanguageExt;

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
        public IEnumerable<Subject> GetAllSubjects()
        {
            return _studyImmutableDataRepository.GetAllSubjects();
        }

        public IEnumerable<Subject> GetSubjectsForStream(int streamId)
        {
            return _subjectForGroupRepository.GetSubjectsForGroup(GetGroupFromStream(streamId).Id);
        }

        public IEnumerable<Subject> GetSubjectsForStreamAndSemester(int streamId, StudySemester semester)
        {
            return _subjectForGroupRepository.GetSubjectsForGroupAndSemester(GetGroupFromStream(streamId).Id, semester);
        }

        public IEnumerable<StudyGroup> GetAllGroups()
        {
            return _studyImmutableDataRepository.GetAllGroups();
        }

        public IEnumerable<StudyGroup> GetGroupsForStream(int streamId)
        {
            return _studyImmutableDataRepository.GetGroupsForStream(streamId);
        }

        public IEnumerable<StudyGroup> GetGroupsForSubject(int subjectId)
        {
            return _subjectForGroupRepository.GetStudyGroupsForSubject(subjectId);
        }

        public IEnumerable<SubjectActivity> GetStudentsRatings(int? subjectId = null, int? streamId = null, int? groupId = null, StudySemester? semester = null)
        {
            if (streamId == null && groupId == null)
            {
                throw new ValueIsNullException();
            }

            semester ??= GetCurrentSemester();

            List<SubjectActivity> result;
            if (subjectId != null)
                result = GetStudentsRatingsForSubject((int) subjectId, streamId, groupId, (StudySemester) semester);
            else
                result = GetStudentsRatingsForAllSubjects(streamId, groupId, (StudySemester) semester);

            return result.OrderByDescending(s => s.Points);
        }

        private List<SubjectActivity> GetStudentsRatingsForSubject(int subjectId, int? streamId, int? groupId, StudySemester semester)
        {
            var result = new List<SubjectActivity>();

            if (groupId != null)
            {
                var subjectForGroup = _subjectForGroupRepository.GetSubjectForGroupForSubjectAndSemester(subjectId, semester).SingleOrDefault(g => g.Id == groupId);
                result = GetStudentsFromGroupRatingsForSubject(subjectForGroup);
            }
            else
            {
                foreach (var subjectForGroup in _subjectForGroupRepository.GetSubjectForGroupForStream((int) streamId))
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
                    _subjectActivityRepository.GetSubjectActivityForStudentAndSubjectForGroup(student.Id,
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

        private List<SubjectActivity> GetStudentsRatingsForAllSubjects(int? streamId, int? groupId, StudySemester semester)
        {
            var result = new List<SubjectActivity>();

            if (groupId != null)
            {
                foreach (var subject in _subjectForGroupRepository.GetSubjectsForGroup((int) groupId))
                {
                    result.AddRange(GetStudentsRatingsForSubject(subject.Id, streamId, groupId, semester));
                }
            }
            else
            {
                foreach (var subject in _subjectForGroupRepository.GetSubjectsForStream((int) streamId))
                {
                    result.AddRange(GetStudentsRatingsForSubject(subject.Id, streamId, null, semester));
                }
            }

            return result;
        }

        private static StudySemester GetCurrentSemester()
        {
            throw new NotImplementedException();
        }

        private StudyGroup GetGroupFromStream(int streamId)
        {
            return GetGroupsForStream(streamId).First();
        }
    }
}

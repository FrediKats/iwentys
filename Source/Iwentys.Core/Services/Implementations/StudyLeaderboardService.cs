using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Types;

namespace Iwentys.Core.Services.Implementations
{
    class StudyLeaderboardService : IStudyLeaderboardService
    {
        private IwentysDbContext _dbContext;
        private ISubjectForGroupRepository _subjectForGroupRepository;
        public StudyLeaderboardService(IwentysDbContext dbContext, ISubjectForGroupRepository subjectForGroupRepository)
        {
            _dbContext = dbContext;
            _subjectForGroupRepository = subjectForGroupRepository;
        }
        public IEnumerable<Subject> GetAllSubjects()
        {
            return _dbContext.Subjects;
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
            return _dbContext.StudyGroups;
        }

        public IEnumerable<StudyGroup> GetGroupsForStream(int streamId)
        {
            return _dbContext.StudyStreams.First(s => s.Id == streamId).Groups;
        }

        public IEnumerable<StudyGroup> GetGroupsForSubject(int subjectId)
        {
            return _subjectForGroupRepository.GetStudyGroupsForSubject(subjectId);
        }

        private StudyGroup GetGroupFromStream(int streamId)
        {
            return GetGroupsForStream(streamId).First();
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Types;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories.Implementations
{
    class SubjectForGroupRepository : ISubjectForGroupRepository
    {
        private readonly IwentysDbContext _dbContext;

        public SubjectForGroupRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public SubjectForGroup Create(SubjectForGroup entity)
        {
            EntityEntry<SubjectForGroup> createdEntity = _dbContext.SubjectForGroups.Add(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public IQueryable<SubjectForGroup> Read()
        {
            return _dbContext.SubjectForGroups;
        }

        public SubjectForGroup ReadById(int key)
        {
            return _dbContext.SubjectForGroups.Find(key);
        }

        public SubjectForGroup Update(SubjectForGroup entity)
        {
            EntityEntry<SubjectForGroup> createdEntity = _dbContext.SubjectForGroups.Update(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public void Delete(int key)
        {
            SubjectForGroup subjectForGroup = this.Get(key);
            _dbContext.SubjectForGroups.Remove(subjectForGroup);
            _dbContext.SaveChanges();
        }

        public IEnumerable<Subject> GetSubjectsForGroup(int groupId)
        {
            return Read().Where(s => s.StudyGroupId == groupId).Select(s => s.Subject);
        }

        public IEnumerable<Subject> GetSubjectsForGroupAndSemester(int groupId, StudySemester semester)
        {
            return Read().Where(s => s.StudyGroupId == groupId && s.StudySemester == semester)
                .Select(s => s.Subject);
        }

        public IEnumerable<SubjectForGroup> GetSubjectForGroupForSubject(int subjectId)
        {
            return Read().Where(s => s.SubjectId == subjectId);
        }

        public IEnumerable<SubjectForGroup> GetSubjectForGroupForSubjectAndSemester(int subjectId, StudySemester semester)
        {
            return GetSubjectForGroupForSubject(subjectId).Where(s => s.StudySemester == semester);
        }

        public IEnumerable<StudyGroup> GetStudyGroupsForSubject(int subjectId)
        {
            return GetSubjectForGroupForSubject(subjectId).Select(s => s.StudyGroup);
        }

        public IEnumerable<StudyGroup> GetStudyGroupsForSubjectAndSemester(int subjectId, StudySemester semester)
        {
            return GetSubjectForGroupForSubjectAndSemester(subjectId, semester).Select(s => s.StudyGroup);
        }

        public IEnumerable<SubjectForGroup> GetSubjectForGroupForStream(int streamId)
        {
            var groupsFromStream = _dbContext.StudyStreams.Find(streamId).Groups;
            return Read().Where(s => groupsFromStream.Any(g => g.Id == s.StudyGroupId));
        }

        public IEnumerable<Subject> GetSubjectsForStream(int streamId)
        {
            return GetSubjectForGroupForStream(streamId).Select(s => s.Subject);
        }
    }
}

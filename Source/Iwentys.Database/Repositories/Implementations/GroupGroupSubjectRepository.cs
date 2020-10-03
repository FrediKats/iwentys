using System.Collections.Generic;
using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models;
using Iwentys.Models.Entities.Study;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories.Implementations
{
    public class GroupGroupSubjectRepository : IGroupSubjectRepository
    {
        private readonly IwentysDbContext _dbContext;

        public GroupGroupSubjectRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<GroupSubjectEntity> Read()
        {
            return _dbContext.GroupSubjects
                .Include(s => s.StudyGroup)
                .Include(s => s.Subject);
        }

        public GroupSubjectEntity ReadById(int key)
        {
            return _dbContext.GroupSubjects.Find(key);
        }

        public GroupSubjectEntity Update(GroupSubjectEntity entity)
        {
            EntityEntry<GroupSubjectEntity> createdEntity = _dbContext.GroupSubjects.Update(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public void Delete(int key)
        {
            GroupSubjectEntity groupSubjectEntity = this.Get(key);
            _dbContext.GroupSubjects.Remove(groupSubjectEntity);
            _dbContext.SaveChanges();
        }

        public IEnumerable<GroupSubjectEntity> GetSubjectForGroupForDto(StudySearchParameters searchParameters)
        {
            IQueryable<GroupSubjectEntity> query = Read();

            if (searchParameters.GroupId != null)
                query = query.Where(s => s.StudyGroupId == searchParameters.GroupId.Value);

            if (searchParameters.StudySemester != null)
                query = query.Where(s => s.StudySemester == searchParameters.StudySemester.Value);

            if (searchParameters.SubjectId != null)
                query = query.Where(s => s.SubjectId == searchParameters.SubjectId.Value);

            if (searchParameters.CourseId != null)
                query = query.Where(gs => gs.StudyGroup.StudyCourseId == searchParameters.CourseId);

            return query;
        }

        public IEnumerable<SubjectEntity> GetSubjectsForDto(StudySearchParameters searchParameters)
        {
            return GetSubjectForGroupForDto(searchParameters).Select(s => s.Subject);
        }

        public IEnumerable<StudyGroupEntity> GetStudyGroupsForDto(int? courseId)
        {
            IQueryable<StudyGroupEntity> query = _dbContext.StudyGroups;

            if (courseId != null)
                query = query.Where(g => g.StudyCourseId == courseId);

            return query;
        }

        public GroupSubjectEntity Create(GroupSubjectEntity entity)
        {
            EntityEntry<GroupSubjectEntity> createdEntity = _dbContext.GroupSubjects.Add(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }
    }
}
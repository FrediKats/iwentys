using System.Collections.Generic;
using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Database.Tools;
using Iwentys.Models;
using Iwentys.Models.Entities.Study;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories.Implementations
{
    public class SubjectActivityRepository : ISubjectActivityRepository
    {
        private readonly IwentysDbContext _dbContext;

        public SubjectActivityRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public SubjectActivityEntity Create(SubjectActivityEntity entity)
        {
            EntityEntry<SubjectActivityEntity> createdEntity = _dbContext.SubjectActivities.Add(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public IQueryable<SubjectActivityEntity> Read()
        {
            return _dbContext.SubjectActivities
                .Include(s => s.Student)
                .Include(s => s.GroupSubject);
        }

        public SubjectActivityEntity ReadById(int key)
        {
            return _dbContext.SubjectActivities.Find(key);
        }

        public SubjectActivityEntity Update(SubjectActivityEntity entity)
        {
            EntityEntry<SubjectActivityEntity> createdEntity = _dbContext.SubjectActivities.Update(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public void Delete(int key)
        {
            SubjectActivityEntity activity = this.Get(key);
            _dbContext.SubjectActivities.Remove(activity);
            _dbContext.SaveChanges();
        }

        public SubjectActivityEntity GetActivityForStudentAndSubject(int studentId, int subjectForGroupId)
        {
            return Read().FirstOrDefault(s => s.StudentId == studentId && s.GroupSubjectEntityId == subjectForGroupId);
        }

        public IEnumerable<SubjectActivityEntity> GetStudentActivities(StudySearchParameters searchParameters)
        {
            var query = Read()
                .Join(_dbContext.StudyGroups,
                    st => st.Student.GroupId,
                    sg => sg.Id,
                    (subjectActivity, group) => new {SubjectActivity = subjectActivity, Group = group})
                .Join(_dbContext.GroupSubjects,
                    st => st.SubjectActivity.GroupSubjectEntityId,
                    sg => sg.Id,
                    (_, sg) => new {_.SubjectActivity, _.Group, SubjectForGroup = sg});

            query = query
                .WhereIf(searchParameters.GroupId, (a, id) => a.Group.Id == id)
                .WhereIf(searchParameters.SubjectId, (a, id) => a.SubjectForGroup.SubjectId == id)
                .WhereIf(searchParameters.CourseId, (a, id) => a.Group.StudyCourseId == id)
                .WhereIf(searchParameters.StudySemester, (a, id) => a.SubjectForGroup.StudySemester == id);

            return query.Select(_ => _.SubjectActivity);
        }
    }
}
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
                .Include(s => s.Student);
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

            if (searchParameters.GroupId != null) query = query.Where(_ => _.Group.Id == searchParameters.GroupId);
            if (searchParameters.SubjectId != null) query = query.Where(_ => _.SubjectForGroup.SubjectId == searchParameters.SubjectId);
            if (searchParameters.CourseId != null) query = query.Where(_ => _.Group.StudyCourseId == searchParameters.CourseId);

            if (searchParameters.StudySemester != null) query = query.Where(_ => _.SubjectForGroup.StudySemester == searchParameters.StudySemester);

            return query.Select(_ => _.SubjectActivity);
        }
    }
}
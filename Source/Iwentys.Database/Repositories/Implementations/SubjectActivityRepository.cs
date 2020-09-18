using System.Collections.Generic;
using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Transferable.Study;
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
            EntityEntry <SubjectActivityEntity> createdEntity = _dbContext.SubjectActivities.Add(entity);
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

        public IEnumerable<SubjectActivityEntity> GetStudentActivities(StudySearchDto searchDto)
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

            if (searchDto.GroupId != null)
            {
                query = query.Where(_ => _.Group.Id == searchDto.GroupId);
            }
            if (searchDto.SubjectId != null)
            {
                query = query.Where(_ => _.SubjectForGroup.SubjectId == searchDto.SubjectId);
            }
            if (searchDto.CourseId != null)
            {
                query = query.Where(_ => _.Group.StudyCourseId == searchDto.CourseId);
            }

            if (searchDto.StudySemester != null)
            {
                query = query.Where(_ => _.SubjectForGroup.StudySemester == searchDto.StudySemester);
            }

            return query.Select(_ => _.SubjectActivity);
        }
    }
}

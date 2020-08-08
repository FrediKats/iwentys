using System.Collections.Generic;
using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Transferable.Study;
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
        public SubjectActivity Create(SubjectActivity entity)
        {
            EntityEntry <SubjectActivity> createdEntity = _dbContext.SubjectActivities.Add(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public IQueryable<SubjectActivity> Read()
        {
            return _dbContext.SubjectActivities;
        }

        public SubjectActivity ReadById(int key)
        {
            return _dbContext.SubjectActivities.Find(key);
        }

        public SubjectActivity Update(SubjectActivity entity)
        {
            EntityEntry<SubjectActivity> createdEntity = _dbContext.SubjectActivities.Update(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public void Delete(int key)
        {
            SubjectActivity activity = this.Get(key);
            _dbContext.SubjectActivities.Remove(activity);
            _dbContext.SaveChanges();
        }

        public SubjectActivity GetActivityForStudentAndSubject(int studentId, int subjectForGroupId)
        {
            return Read().FirstOrDefault(s => s.StudentId == studentId && s.SubjectForGroupId == subjectForGroupId);
        }

        public IEnumerable<SubjectActivity> GetStudentActivities(StudySearchDto searchDto)
        {
            var query = Read().Join(_dbContext.StudyGroups, st => st.Student.Group, sg => sg.NamePattern, 
                (subjectActivity, group) => new {SubjectActivity = subjectActivity, Group = group}).Join(_dbContext.SubjectForGroups, st => st.Group.Id, sg => sg.StudyGroupId,
                (_, sg) => new {_.SubjectActivity, _.Group, SubjectForGroup = sg});

            if (searchDto.GroupId != null)
            {
                query = query.Where(_ => _.Group.Id == searchDto.GroupId);
            }
            if (searchDto.SubjectId != null)
            {
                query = query.Where(_ => _.SubjectForGroup.SubjectId == searchDto.SubjectId);
            }
            if (searchDto.StreamId != null)
            {
                query = query.Where(_ => _.Group.StudyStreamId == searchDto.StreamId);
            }

            if (searchDto.StudySemester != null)
            {
                query = query.Where(_ => _.SubjectForGroup.StudySemester == searchDto.StudySemester);
            }

            return query.Select(_ => _.SubjectActivity);
        }
    }
}

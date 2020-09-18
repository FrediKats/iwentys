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
    public class GroupGroupSubjectRepository : IGroupSubjectRepository
    {
        private readonly IwentysDbContext _dbContext;

        public GroupGroupSubjectRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public GroupSubjectEntity Create(GroupSubjectEntity entity)
        {
            EntityEntry<GroupSubjectEntity> createdEntity = _dbContext.GroupSubjects.Add(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
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

        public IEnumerable<GroupSubjectEntity> GetSubjectForGroupForDto(StudySearchDto searchDto)
        {
            var query = Read();
            if (searchDto.GroupId != null)
            {
                query = query.Where(s => s.StudyGroupId == searchDto.GroupId.Value);
            }

            if (searchDto.StudySemester != null)
            {
                query = query.Where(s => s.StudySemester == searchDto.StudySemester.Value);
            }

            if (searchDto.SubjectId != null)
            {
                query = query.Where(s => s.SubjectId == searchDto.SubjectId.Value);
            }

            if (searchDto.CourseId != null)
            {
                //TODO: it will not work lol
                List<StudyGroupEntity> courseGroups = _dbContext.StudyGroups.Where(g => g.StudyCourseId == searchDto.CourseId).ToList();
                query = query.Where(s => courseGroups.Any(g => g.Id == s.StudyGroupId));
            }

            return query;
        }

        public IEnumerable<SubjectEntity> GetSubjectsForDto(StudySearchDto searchDto)
        {
            return GetSubjectForGroupForDto(searchDto).Select(s => s.Subject);
        }

        public IEnumerable<StudyGroupEntity> GetStudyGroupsForDto(StudySearchDto searchDto)
        {
            return GetSubjectForGroupForDto(searchDto).Select(s => s.StudyGroup);
        }
    }
}

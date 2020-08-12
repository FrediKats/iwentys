using System.Collections.Generic;
using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Transferable.Study;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories.Implementations
{
    public class SubjectForGroupRepository : ISubjectForGroupRepository
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

        public IEnumerable<SubjectForGroup> GetSubjectForGroupForDto(StudySearchDto searchDto)
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

            if (searchDto.StreamId != null)
            {
                var groupsFromStream = _dbContext.StudyStreams.Find(searchDto.StreamId.Value).Groups;
                query = query.Where(s => groupsFromStream.Any(g => g.Id == s.StudyGroupId));
            }

            return query;
        }

        public IEnumerable<Subject> GetSubjectsForDto(StudySearchDto searchDto)
        {
            return GetSubjectForGroupForDto(searchDto).Select(s => s.Subject);
        }

        public IEnumerable<StudyGroup> GetStudyGroupsForDto(StudySearchDto searchDto)
        {
            return GetSubjectForGroupForDto(searchDto).Select(s => s.StudyGroup);
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Study;

namespace Iwentys.Database.Repositories.Implementations
{
    public class StudyImmutableDataRepository : IStudyImmutableDataRepository
    {
        private readonly IwentysDbContext _dbContext;

        public StudyImmutableDataRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Subject> GetAllSubjects()
        {
            return _dbContext.Subjects;
        }

        public IQueryable<StudyGroup> GetAllGroups()
        {
            return _dbContext.StudyGroups;
        }

        public IEnumerable<StudyGroup> GetGroupsForStream(int streamId)
        {
            return _dbContext.StudyGroups.Where(g => g.StudyStreamId == streamId).ToList();
        }

        public IEnumerable<Student> GetStudentsForGroup(string groupName)
        {
            return _dbContext.Students.Where(s => s.Group.GroupName == groupName);
        }
    }
}

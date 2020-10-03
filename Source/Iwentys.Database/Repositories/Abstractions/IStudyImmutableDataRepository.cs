using System.Collections.Generic;
using System.Linq;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Study;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface IStudyImmutableDataRepository
    {
        IQueryable<SubjectEntity> GetAllSubjects();
        IQueryable<StudyGroupEntity> GetAllGroups();
        IEnumerable<StudyGroupEntity> GetCourseGroups(int courseId);
        IEnumerable<StudentEntity> GetStudentsForGroup(string groupName);
    }
}
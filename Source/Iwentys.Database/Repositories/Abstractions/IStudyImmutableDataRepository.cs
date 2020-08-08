using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Study;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface IStudyImmutableDataRepository
    {
        IQueryable<Subject> GetAllSubjects();
        IQueryable<StudyGroup> GetAllGroups();
        IEnumerable<StudyGroup> GetGroupsForStream(int streamId);
        IEnumerable<Student> GetStudentsForGroup(string group);
    }
}

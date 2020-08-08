using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Types;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface ISubjectForGroupRepository : IGenericRepository<SubjectForGroup, int>
    {
        IQueryable<Subject> GetSubjectsForGroup(int groupId);
        IQueryable<Subject> GetSubjectsForGroupAndSemester(int groupId, StudySemester semester);
        IQueryable<StudyGroup> GetStudyGroupsForSubject(int subjectId);
    }
}

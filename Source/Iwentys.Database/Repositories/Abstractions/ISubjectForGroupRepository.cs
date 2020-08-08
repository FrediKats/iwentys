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
        IEnumerable<Subject> GetSubjectsForGroup(int groupId);
        IEnumerable<Subject> GetSubjectsForGroupAndSemester(int groupId, StudySemester semester);
        IEnumerable<SubjectForGroup> GetSubjectForGroupForSubject(int subjectId);
        IEnumerable<SubjectForGroup> GetSubjectForGroupForSubjectAndSemester(int subjectId, StudySemester semester);
        IEnumerable<StudyGroup> GetStudyGroupsForSubject(int subjectId);
        IEnumerable<StudyGroup> GetStudyGroupsForSubjectAndSemester(int subjectId, StudySemester semester);
        IEnumerable<SubjectForGroup> GetSubjectForGroupForStream(int streamId);
        IEnumerable<Subject> GetSubjectsForStream(int streamId);
    }
}

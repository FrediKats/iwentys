using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Types;

namespace Iwentys.Core.Services.Abstractions
{
    interface IStudyLeaderboardService
    {
        IEnumerable<Subject> GetAllSubjects();
        IEnumerable<Subject> GetSubjectsForStream(int streamId);
        IEnumerable<Subject> GetSubjectsForStreamAndSemester(int streamId, StudySemester semester);
        IEnumerable<StudyGroup> GetAllGroups();
        IEnumerable<StudyGroup> GetGroupsForStream(int streamId);
        IEnumerable<StudyGroup> GetGroupsForSubject(int subjectId);

    }
}

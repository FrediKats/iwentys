using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Transferable.Study;
using Iwentys.Models.Types;

namespace Iwentys.Core.Services.Abstractions
{
    public interface IStudyLeaderboardService
    {
        IEnumerable<Subject> GetSubjectsForDto(StudySearchDto searchDto);
        IEnumerable<StudyGroup> GetStudyGroupsForDto(StudySearchDto searchDto);
        IEnumerable<SubjectActivity> GetStudentsRatings(StudySearchDto searchDto);
    }
}

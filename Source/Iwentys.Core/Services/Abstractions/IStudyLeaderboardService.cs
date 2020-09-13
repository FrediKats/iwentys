using System.Collections.Generic;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Transferable.Study;

namespace Iwentys.Core.Services.Abstractions
{
    public interface IStudyLeaderboardService
    {
        IEnumerable<SubjectEntity> GetSubjectsForDto(StudySearchDto searchDto);
        IEnumerable<StudyGroupEntity> GetStudyGroupsForDto(StudySearchDto searchDto);
        List<StudyLeaderboardRow> GetStudentsRatings(StudySearchDto searchDto);
    }
}

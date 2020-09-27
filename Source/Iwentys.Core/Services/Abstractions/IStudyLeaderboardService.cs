using System.Collections.Generic;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Transferable.Study;

namespace Iwentys.Core.Services.Abstractions
{
    public interface IStudyLeaderboardService
    {
        List<SubjectEntity> GetSubjectsForDto(StudySearchDto searchDto);
        List<StudyGroupEntity> GetStudyGroupsForDto(int? courseId);
        List<StudyLeaderboardRow> GetStudentsRatings(StudySearchDto searchDto);
    }
}

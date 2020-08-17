using System.Collections.Generic;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Transferable.Study;

namespace Iwentys.Core.Services.Abstractions
{
    public interface IStudyLeaderboardService
    {
        IEnumerable<Subject> GetSubjectsForDto(StudySearchDto searchDto);
        IEnumerable<StudyGroup> GetStudyGroupsForDto(StudySearchDto searchDto);
        List<StudyLeaderboardRow> GetStudentsRatings(StudySearchDto searchDto);
    }
}

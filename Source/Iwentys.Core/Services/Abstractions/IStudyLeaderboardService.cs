using System.Collections.Generic;
using Iwentys.Models;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Transferable.Study;

namespace Iwentys.Core.Services.Abstractions
{
    public interface IStudyLeaderboardService
    {
        List<SubjectEntity> GetSubjectsForDto(StudySearchParameters searchParameters);
        List<StudyGroupEntity> GetStudyGroupsForDto(int? courseId);
        List<StudyLeaderboardRow> GetStudentsRatings(StudySearchParameters searchParameters);
        List<StudyLeaderboardRow> GetCodingRating(int? courseId);
    }
}

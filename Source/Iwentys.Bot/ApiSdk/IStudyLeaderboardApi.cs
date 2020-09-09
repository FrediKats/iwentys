using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Models.Transferable.Study;
using Iwentys.Models.Types;
using Refit;

namespace Iwentys.ClientBot.ApiSdk
{
    public interface IStudyLeaderboardApi
    {
        [Get("/api/StudyLeaderboard/GetStudentsRating")]
        public Task<List<StudyLeaderboardRow>> GetStudentsRating(int? subjectId, int? streamId, int? groupId, StudySemester? semester);
    }
}
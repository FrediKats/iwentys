using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Models.Entities.Study;
using Refit;

namespace Iwentys.ClientBot.ApiSdk
{
    public interface IStudyGroupApi
    {
        [Get("/api/StudyGroup")]
        Task<List<StudyGroupEntity>> Get(int? courseId, int? subjectId);
    }
}
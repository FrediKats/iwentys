using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Models.Entities.Study;
using Refit;

namespace Iwentys.ClientBot.ApiSdk
{
    public interface ISubjectApi
    {
        [Get("/api/Subject")]
        Task<List<SubjectEntity>> Get();
    }
}
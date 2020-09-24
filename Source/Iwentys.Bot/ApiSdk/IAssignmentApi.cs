using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Models.Transferable;
using Refit;

namespace Iwentys.ClientBot.ApiSdk
{
    public interface IAssignmentApi
    {
        [Get("/api/Assignment")]
        Task<List<AssignmentInfoDto>> Get();

        [Post("/api/Assignment")]
        Task<List<AssignmentInfoDto>> Create([Body] AssignmentCreateDto assignmentCreateDto);
    }
}
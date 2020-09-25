using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Models.Transferable.Students;
using Refit;

namespace Iwentys.ClientBot.ApiSdk
{
    public interface IStudentApi
    {
        [Get("/api/student")]
        Task<List<StudentFullProfileDto>> Get();

        [Get("/api/student/{id}")]
        Task<StudentFullProfileDto> Get(int id);

        [Get("/api/student/for-group/{groupName}")]
        Task<List<StudentFullProfileDto>> Get(string groupName);

        [Put("/api/student")]
        Task<StudentFullProfileDto> Update(StudentUpdateDto studentUpdateDto);
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Models.Transferable.Students;
using Refit;

namespace Iwentys.ApiSdk
{
    public interface IIwentysStudentApi
    {
        [Get("/api/student")]
        Task<IEnumerable<StudentFullProfileDto>> Get();

        [Get("/api/student/{id}")]
        Task<StudentFullProfileDto> Get(int id);

        [Post("/api/student")]
        Task<StudentFullProfileDto> Update(StudentUpdateDto studentUpdateDto);
    }
}
using System.Collections.Generic;
using Iwentys.Models.Transferable.Students;

namespace Iwentys.ClientBot.ApiIntegration
{
    public interface IIwentysStudentApi
    {
        IEnumerable<StudentFullProfileDto> Get();
        StudentFullProfileDto Get(int id);
    }
}
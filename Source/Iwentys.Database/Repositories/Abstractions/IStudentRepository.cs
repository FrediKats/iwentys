using Iwentys.Models.Entities;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface IStudentRepository : IGenericRepository<StudentEntity, int>
    {
        StudentEntity Create(StudentEntity student);
    }
}
using Iwentys.Models.Entities;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface IStudentRepository : IGenericRepository<Student, int>
    {
        Student Create(Student student);
    }
}
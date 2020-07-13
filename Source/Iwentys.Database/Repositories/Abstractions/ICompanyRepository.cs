using Iwentys.Models.Entities;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface ICompanyRepository : IGenericRepository<Company, int>
    {
        Student[] ReadWorkers(Company company);
        CompanyWorker[] ReadWorkerRequest();
        void AddCompanyWorkerRequest(Company company, Student worker);
        void ApproveRequest(Student user);
    }
}
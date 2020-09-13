using Iwentys.Models.Entities;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface ICompanyRepository : IGenericRepository<Company, int>
    {
        //TODO: rework
        Company Create(Company company);

        Student[] ReadWorkers(Company company);
        CompanyWorker[] ReadWorkerRequest();
        void AddCompanyWorkerRequest(Company company, Student worker);
        void ApproveRequest(Student user);
    }
}
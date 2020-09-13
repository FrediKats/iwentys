using Iwentys.Models.Entities;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface ICompanyRepository : IGenericRepository<Company, int>
    {
        //TODO: rework
        Company Create(Company company);

        StudentEntity[] ReadWorkers(Company company);
        CompanyWorker[] ReadWorkerRequest();
        void AddCompanyWorkerRequest(Company company, StudentEntity worker);
        void ApproveRequest(StudentEntity user);
    }
}
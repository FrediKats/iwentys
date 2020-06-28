using Iwentys.Models.Entities;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface ICompanyRepository : IGenericRepository<Company, int>
    {
        UserProfile[] ReadWorkers(Company company);
        CompanyWorker[] ReadWorkerRequest();
        void AddCompanyWorkerRequest(Company company, UserProfile worker);
        void ApproveRequest(UserProfile user);
    }
}
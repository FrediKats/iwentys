using Iwentys.Models.Entities;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface ICompanyRepository : IGenericRepository<CompanyEntity, int>
    {
        //TODO: rework
        CompanyEntity Create(CompanyEntity companyEntity);

        StudentEntity[] ReadWorkers(CompanyEntity companyEntity);
        CompanyWorkerEntity[] ReadWorkerRequest();
        void AddCompanyWorkerRequest(CompanyEntity companyEntity, StudentEntity worker);
        void ApproveRequest(StudentEntity user);
    }
}
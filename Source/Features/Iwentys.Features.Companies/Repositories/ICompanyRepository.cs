using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Models.Entities;

namespace Iwentys.Features.Companies.Repositories
{
    public interface ICompanyRepository : IGenericRepository<CompanyEntity, int>
    {
        Task<CompanyEntity> CreateAsync(CompanyEntity entity);
        Task<List<StudentEntity>> ReadWorkersAsync(CompanyEntity companyEntity);
        Task<List<CompanyWorkerEntity>> ReadWorkerRequestAsync();
        Task AddCompanyWorkerRequestAsync(CompanyEntity companyEntity, StudentEntity worker);
        Task ApproveRequestAsync(StudentEntity user);
    }
}
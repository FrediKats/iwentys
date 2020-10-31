using System.Threading.Tasks;
using Iwentys.Core.DomainModel;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories;
using Iwentys.Models.Entities;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable.Companies;

namespace Iwentys.Core.Services
{
    public class CompanyService
    {
        private readonly DatabaseAccessor _database;

        public CompanyService(DatabaseAccessor database)
        {
            _database = database;
        }

        public CompanyInfoResponse[] Get()
        {
            return _database.Company.Read().SelectToArray(WrapToDto);
        }

        public async Task<CompanyInfoResponse> Get(int id)
        {
            CompanyEntity company = await _database.Company.ReadByIdAsync(id);
            return WrapToDto(company);
        }

        public CompanyWorkRequestDto[] GetCompanyWorkRequest()
        {
            return _database.Company
                .ReadWorkerRequest()
                .SelectToArray(cw => cw.To(CompanyWorkRequestDto.Create));
        }

        public async Task RequestAdding(int companyId, int userId)
        {
            CompanyEntity companyEntity = await _database.Company.GetAsync(companyId);
            StudentEntity profile = await _database.Student.GetAsync(userId);
            _database.Company.AddCompanyWorkerRequest(companyEntity, profile);
        }

        public async Task ApproveAdding(int userId, int adminId)
        {
            var student = await _database.Student
                .GetAsync(adminId);

            student.EnsureIsAdmin();

            StudentEntity user = await _database.Student.GetAsync(userId);

            _database.Company.ApproveRequest(user);
        }

        private CompanyInfoResponse WrapToDto(CompanyEntity companyEntity)
        {
            StudentEntity[] workers = _database.Company.ReadWorkers(companyEntity);
            return CompanyInfoResponse.Create(companyEntity, workers);
        }
    }
}
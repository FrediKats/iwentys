using Iwentys.Core.DomainModel;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories;
using Iwentys.Models.Entities;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable.Companies;

namespace Iwentys.Core.Services.Implementations
{
    public class CompanyService : ICompanyService
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

        public CompanyInfoResponse Get(int id)
        {
            return _database.Company.ReadById(id).To(WrapToDto);
        }

        public CompanyWorkRequestDto[] GetCompanyWorkRequest()
        {
            return _database.Company
                .ReadWorkerRequest()
                .SelectToArray(cw => cw.To(CompanyWorkRequestDto.Create));
        }

        public void RequestAdding(int companyId, int userId)
        {
            CompanyEntity companyEntity = _database.Company.Get(companyId);
            StudentEntity profile = _database.Student.Get(userId);
            _database.Company.AddCompanyWorkerRequest(companyEntity, profile);
        }

        public void ApproveAdding(int userId, int adminId)
        {
            _database.Student
                .Get(adminId)
                .EnsureIsAdmin();

            StudentEntity user = _database.Student.Get(userId);

            _database.Company.ApproveRequest(user);
        }

        private CompanyInfoResponse WrapToDto(CompanyEntity companyEntity)
        {
            StudentEntity[] workers = _database.Company.ReadWorkers(companyEntity);
            return CompanyInfoResponse.Create(companyEntity, workers);
        }
    }
}
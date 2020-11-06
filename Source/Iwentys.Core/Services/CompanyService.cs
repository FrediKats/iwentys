using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Core.DomainModel;
using Iwentys.Database.Context;
using Iwentys.Models.Entities;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable.Companies;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Core.Services
{
    public class CompanyService
    {
        private readonly DatabaseAccessor _database;

        public CompanyService(DatabaseAccessor database)
        {
            _database = database;
        }

        public async Task<List<CompanyInfoResponse>> Get()
        {
            var info = await _database.Company.Read().ToListAsync();
            return info.SelectToList(entity => WrapToDto(entity).Result);
        }

        public async Task<CompanyInfoResponse> Get(int id)
        {
            CompanyEntity company = await _database.Company.ReadByIdAsync(id);
            return await WrapToDto(company);
        }

        public async Task<List<CompanyWorkRequestDto>> GetCompanyWorkRequest()
        {
            List<CompanyWorkerEntity> workers = await _database.Company.ReadWorkerRequestAsync();
            return workers.SelectToList(cw => cw.To(CompanyWorkRequestDto.Create));
        }

        public async Task RequestAdding(int companyId, int userId)
        {
            CompanyEntity companyEntity = await _database.Company.GetAsync(companyId);
            StudentEntity profile = await _database.Student.GetAsync(userId);
            await _database.Company.AddCompanyWorkerRequestAsync(companyEntity, profile);
        }

        public async Task ApproveAdding(int userId, int adminId)
        {
            var student = await _database.Student
                .GetAsync(adminId);

            student.EnsureIsAdmin();

            StudentEntity user = await _database.Student.GetAsync(userId);

            await _database.Company.ApproveRequestAsync(user);
        }

        private async Task<CompanyInfoResponse> WrapToDto(CompanyEntity companyEntity)
        {
            List<StudentEntity> workers = await _database.Company.ReadWorkersAsync(companyEntity);
            return CompanyInfoResponse.Create(companyEntity, workers);
        }
    }
}
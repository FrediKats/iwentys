using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Features.Companies.Entities;
using Iwentys.Features.Companies.Repositories;
using Iwentys.Features.Companies.ViewModels;
using Iwentys.Features.StudentFeature;
using Iwentys.Features.StudentFeature.Repositories;
using Iwentys.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Companies.Services
{
    public class CompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IStudentRepository _studentRepository;

        public CompanyService(ICompanyRepository companyRepository, IStudentRepository studentRepository)
        {
            _companyRepository = companyRepository;
            _studentRepository = studentRepository;
        }

        public async Task<List<CompanyViewModel>> Get()
        {
            var info = await _companyRepository.Read().ToListAsync();
            return info.SelectToList(entity => WrapToDto(entity).Result);
        }

        public async Task<CompanyViewModel> Get(int id)
        {
            CompanyEntity company = await _companyRepository.ReadByIdAsync(id);
            return await WrapToDto(company);
        }

        public async Task<List<CompanyWorkRequestViewModel>> GetCompanyWorkRequest()
        {
            List<CompanyWorkerEntity> workers = await _companyRepository.ReadWorkerRequestAsync();
            return workers.SelectToList(cw => cw.To(CompanyWorkRequestViewModel.Create));
        }

        public async Task RequestAdding(int companyId, int userId)
        {
            CompanyEntity companyEntity = await _companyRepository.GetAsync(companyId);
            StudentEntity profile = await _studentRepository.GetAsync(userId);
            await _companyRepository.AddCompanyWorkerRequestAsync(companyEntity, profile);
        }

        public async Task ApproveAdding(int userId, int adminId)
        {
            var student = await _studentRepository
                .GetAsync(adminId);

            student.EnsureIsAdmin();

            StudentEntity user = await _studentRepository.GetAsync(userId);

            await _companyRepository.ApproveRequestAsync(user);
        }

        private async Task<CompanyViewModel> WrapToDto(CompanyEntity companyEntity)
        {
            List<StudentEntity> workers = await _companyRepository.ReadWorkersAsync(companyEntity);
            return CompanyViewModel.Create(companyEntity, workers);
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Features.Companies.Entities;
using Iwentys.Features.Companies.Models;
using Iwentys.Features.Companies.Repositories;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Repositories;
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

        public async Task<List<CompanyInfoDto>> Get()
        {
            List<CompanyEntity> info = await _companyRepository.Read().ToListAsync();
            return info.SelectToList(entity => new CompanyInfoDto(entity));
        }

        public async Task<CompanyInfoDto> Get(int id)
        {
            return new CompanyInfoDto(await _companyRepository.ReadByIdAsync(id));
        }

        public async Task<List<CompanyWorkRequestDto>> GetCompanyWorkRequest()
        {
            List<CompanyWorkerEntity> workers = await _companyRepository.ReadWorkerRequestAsync();
            return workers.SelectToList(CompanyWorkRequestDto.Create);
        }

        public async Task RequestAdding(int companyId, int userId)
        {
            CompanyEntity companyEntity = await _companyRepository.GetAsync(companyId);
            StudentEntity profile = await _studentRepository.GetAsync(userId);
            await _companyRepository.AddCompanyWorkerRequestAsync(companyEntity, profile);
        }

        public async Task ApproveAdding(int userId, int adminId)
        {
            StudentEntity student = await _studentRepository
                .GetAsync(adminId);
            student.EnsureIsAdmin();
            StudentEntity user = await _studentRepository.GetAsync(userId);
            
            await _companyRepository.ApproveRequestAsync(user);
        }
    }
}
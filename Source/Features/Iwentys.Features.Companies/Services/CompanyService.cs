using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Common.Tools;
using Iwentys.Features.Companies.Entities;
using Iwentys.Features.Companies.Models;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Companies.Services
{
    public class CompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IGenericRepository<CompanyWorkerEntity> _companyWorkerRepository;
        private readonly IGenericRepository<CompanyEntity> _companyRepository;
        private readonly IGenericRepository<StudentEntity> _studentRepository;

        public CompanyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
            _companyRepository = _unitOfWork.GetRepository<CompanyEntity>();
            _companyWorkerRepository = _unitOfWork.GetRepository<CompanyWorkerEntity>();
            _studentRepository = _unitOfWork.GetRepository<StudentEntity>();
        }

        public async Task<List<CompanyInfoDto>> GetAsync()
        {
            List<CompanyEntity> info = await _companyRepository.Get().ToListAsync();
            return info.SelectToList(entity => new CompanyInfoDto(entity));
        }

        public async Task<CompanyInfoDto> GetAsync(int id)
        {
            return new CompanyInfoDto(await _companyRepository.FindByIdAsync(id));
        }

        public async Task<List<CompanyWorkRequestDto>> GetCompanyWorkRequest()
        {
            return await _companyWorkerRepository
                .Get()
                .Where(CompanyWorkerEntity.IsRequested)
                .Select(CompanyWorkRequestDto.FromEntity)
                .ToListAsync();
        }

        public async Task RequestAdding(int companyId, int userId)
        {
            CompanyEntity companyEntity = await _companyRepository.FindByIdAsync(companyId);
            StudentEntity profile = await _studentRepository.FindByIdAsync(userId);
            
            List<CompanyWorkerEntity> workerRequests = await _companyWorkerRepository.Get().Where(CompanyWorkerEntity.IsRequested).ToListAsync();
            if (workerRequests.Any(r => r.WorkerId == profile.Id))
                throw new InnerLogicException("Student already request adding to company");

            await _companyWorkerRepository.InsertAsync(CompanyWorkerEntity.NewRequest(companyEntity, profile));
            await _unitOfWork.CommitAsync();
        }

        public async Task ApproveAdding(int userId, int adminId)
        {
            StudentEntity student = await _studentRepository.FindByIdAsync(adminId);
            var admin = student.EnsureIsAdmin();

            var companyWorkerEntity = await _companyWorkerRepository.Get().SingleAsync(cw => cw.WorkerId == userId);
            companyWorkerEntity.Approve(admin);
            
            _companyWorkerRepository.Update(companyWorkerEntity);
            await _unitOfWork.CommitAsync();
        }

        //TODO: rework
        public async Task<CompanyEntity> Create(CompanyEntity company)
        {
            await _companyRepository.InsertAsync(company);
            await _unitOfWork.CommitAsync();
            
            return company;
        }
    }
}
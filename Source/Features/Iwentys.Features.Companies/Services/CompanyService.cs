using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Common.Tools;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.AccountManagement.Entities;
using Iwentys.Features.Companies.Entities;
using Iwentys.Features.Companies.Models;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Companies.Services
{
    public class CompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IGenericRepository<CompanyWorker> _companyWorkerRepository;
        private readonly IGenericRepository<Company> _companyRepository;
        private readonly IGenericRepository<IwentysUser> _studentRepository;
        private readonly IGenericRepository<IwentysUser> _iwentysUserRepository;

        public CompanyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
            _companyRepository = _unitOfWork.GetRepository<Company>();
            _companyWorkerRepository = _unitOfWork.GetRepository<CompanyWorker>();
            _studentRepository = _unitOfWork.GetRepository<IwentysUser>();
            _iwentysUserRepository = _unitOfWork.GetRepository<IwentysUser>();
        }

        public async Task<List<CompanyInfoDto>> GetAsync()
        {
            List<Company> info = await _companyRepository.Get().ToListAsync();
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
                .Where(CompanyWorker.IsRequested)
                .Select(CompanyWorkRequestDto.FromEntity)
                .ToListAsync();
        }

        public async Task RequestAdding(int companyId, int userId)
        {
            Company company = await _companyRepository.FindByIdAsync(companyId);
            IwentysUser profile = await _studentRepository.FindByIdAsync(userId);
            
            List<CompanyWorker> workerRequests = await _companyWorkerRepository.Get().Where(CompanyWorker.IsRequested).ToListAsync();
            if (workerRequests.Any(r => r.WorkerId == profile.Id))
                throw new InnerLogicException("Student already request adding to company");

            await _companyWorkerRepository.InsertAsync(CompanyWorker.NewRequest(company, profile));
            await _unitOfWork.CommitAsync();
        }

        public async Task ApproveAdding(AuthorizedUser authorizedAdmin, int userId)
        {
            IwentysUser iwentysUser = await _iwentysUserRepository.GetByIdAsync(authorizedAdmin.Id);
            SystemAdminUser admin = iwentysUser.EnsureIsAdmin();

            var companyWorkerEntity = await _companyWorkerRepository.Get().SingleAsync(cw => cw.WorkerId == userId);
            companyWorkerEntity.Approve(admin);
            
            _companyWorkerRepository.Update(companyWorkerEntity);
            await _unitOfWork.CommitAsync();
        }

        //TODO: rework
        public async Task<Company> Create(Company company)
        {
            await _companyRepository.InsertAsync(company);
            await _unitOfWork.CommitAsync();
            
            return company;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Tools;
using Iwentys.Domain;
using Iwentys.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Extended.Services
{
    public class CompanyService
    {
        private readonly IGenericRepository<Company> _companyRepository;
        private readonly IGenericRepository<CompanyWorker> _companyWorkerRepository;
        private readonly IGenericRepository<IwentysUser> _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CompanyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _companyRepository = _unitOfWork.GetRepository<Company>();
            _companyWorkerRepository = _unitOfWork.GetRepository<CompanyWorker>();
            _userRepository = _unitOfWork.GetRepository<IwentysUser>();
        }

        public async Task<List<CompanyInfoDto>> Get()
        {
            return await _companyRepository
                .Get()
                .Select(entity => new CompanyInfoDto(entity))
                .ToListAsync();
        }

        public async Task<CompanyInfoDto> Get(int id)
        {
            return await _companyRepository
                .GetById(id)
                .To(entity => new CompanyInfoDto(entity));
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
            Company company = await _companyRepository.GetById(companyId);
            IwentysUser profile = await _userRepository.GetById(userId);
            CompanyWorker currentWorkerState = await _companyWorkerRepository.Get().FirstOrDefaultAsync(cw => cw.WorkerId == userId);

            var newRequest = CompanyWorker.NewRequest(company, profile, currentWorkerState);

            _companyWorkerRepository.Insert(newRequest);
            await _unitOfWork.CommitAsync();
        }

        public async Task ApproveAdding(AuthorizedUser authorizedAdmin, int userId)
        {
            IwentysUser iwentysUser = await _userRepository.GetById(authorizedAdmin.Id);
            CompanyWorker companyWorker = await _companyWorkerRepository.Get().SingleAsync(cw => cw.WorkerId == userId);
            
            companyWorker.Approve(iwentysUser);

            _companyWorkerRepository.Update(companyWorker);
            await _unitOfWork.CommitAsync();
        }

        public async Task<CompanyInfoDto> Create(AuthorizedUser initiator, CompanyCreateArguments createArguments)
        {
            IwentysUser creator = await _userRepository.GetById(initiator.Id);

            var company = Company.Create(creator, createArguments);

            _companyRepository.Insert(company);
            await _unitOfWork.CommitAsync();

            return new CompanyInfoDto(company);
        }
    }
}
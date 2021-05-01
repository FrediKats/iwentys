using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Tools;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Extended;
using Iwentys.Domain.Extended.Models;
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
    }
}
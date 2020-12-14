using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Common.Tools;
using Iwentys.Features.Companies.Entities;
using Iwentys.Features.Companies.Enums;
using Iwentys.Features.Companies.Models;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Companies.Services
{
    public class CompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IGenericRepository<CompanyWorkerEntity> _companyWorkerRepository;
        private readonly IGenericRepository<CompanyEntity> _companyRepository;
        private readonly IStudentRepository _studentRepository;

        public CompanyService(IStudentRepository studentRepository, IUnitOfWork unitOfWork)
        {
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
            _companyRepository = _unitOfWork.GetRepository<CompanyEntity>();
            _companyWorkerRepository = _unitOfWork.GetRepository<CompanyWorkerEntity>();
        }

        public async Task<List<CompanyInfoDto>> Get()
        {
            List<CompanyEntity> info = await _companyRepository.GetAsync().ToListAsync();
            return info.SelectToList(entity => new CompanyInfoDto(entity));
        }

        public async Task<CompanyInfoDto> Get(int id)
        {
            return new CompanyInfoDto(await _companyRepository.GetByIdAsync(id));
        }

        public async Task<List<CompanyWorkRequestDto>> GetCompanyWorkRequest()
        {
            //TODO: move where to expression
            List<CompanyWorkerEntity> workerRequests = await _companyWorkerRepository.GetAsync().Where(r => r.Type == CompanyWorkerType.Requested).ToListAsync();
            return workerRequests.SelectToList(CompanyWorkRequestDto.Create);
        }

        public async Task RequestAdding(int companyId, int userId)
        {
            CompanyEntity companyEntity = await _companyRepository.GetByIdAsync(companyId);
            StudentEntity profile = await _studentRepository.GetAsync(userId);
            
            List<CompanyWorkerEntity> workerRequests = await _companyWorkerRepository.GetAsync().Where(r => r.Type == CompanyWorkerType.Requested).ToListAsync();
            if (workerRequests.Any(r => r.WorkerId == profile.Id))
                throw new InnerLogicException("Student already request adding to company");

            await _companyWorkerRepository.InsertAsync(CompanyWorkerEntity.NewRequest(companyEntity, profile));
            await _unitOfWork.CommitAsync();
        }

        public async Task ApproveAdding(int userId, int adminId)
        {
            StudentEntity student = await _studentRepository.GetAsync(adminId);
            student.EnsureIsAdmin();

            var companyWorkerEntity = await _companyWorkerRepository.GetAsync().SingleAsync(cw => cw.WorkerId == userId);
            companyWorkerEntity.Approve();
            
            await _companyWorkerRepository.UpdateAsync(companyWorkerEntity);
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
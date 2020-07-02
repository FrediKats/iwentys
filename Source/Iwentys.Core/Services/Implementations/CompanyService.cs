using Iwentys.Core.DomainModel;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Repositories;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable.Companies;

namespace Iwentys.Core.Services.Implementations
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IStudentRepository _studentRepository;

        public CompanyService(ICompanyRepository companyRepository, IStudentRepository studentRepository)
        {
            _companyRepository = companyRepository;
            _studentRepository = studentRepository;
        }

        public CompanyInfoDto[] Get()
        {
            return _companyRepository.Read().SelectToArray(WrapToDto);
        }

        public CompanyInfoDto Get(int id)
        {
            return _companyRepository.ReadById(id).To(WrapToDto);
        }

        public CompanyWorkRequestDto[] GetCompanyWorkRequest()
        {
            return _companyRepository
                .ReadWorkerRequest()
                .SelectToArray(cw => cw.To(CompanyWorkRequestDto.Create));
        }

        public void RequestAdding(int companyId, int userId)
        {
            Company company = _companyRepository.Get(companyId);
            Student profile = _studentRepository.Get(userId);
            _companyRepository.AddCompanyWorkerRequest(company, profile);
        }

        public void ApproveAdding(int userId, int adminId)
        {
            _studentRepository
                .Get(adminId)
                .EnsureIsAdmin();

            Student user = _studentRepository.Get(userId);

            _companyRepository.ApproveRequest(user);
        }

        private CompanyInfoDto WrapToDto(Company company)
        {
            Student[] workers = _companyRepository.ReadWorkers(company);
            return CompanyInfoDto.Create(company, workers);
        }
    }
}
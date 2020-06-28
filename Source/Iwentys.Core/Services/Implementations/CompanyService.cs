using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Repositories;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.DomainModel;
using Iwentys.Models.Entities;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable.Companies;

namespace Iwentys.Core.Services.Implementations
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserProfileRepository _userProfileRepository;

        public CompanyService(ICompanyRepository companyRepository, IUserProfileRepository userProfileRepository)
        {
            _companyRepository = companyRepository;
            _userProfileRepository = userProfileRepository;
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
            UserProfile profile = _userProfileRepository.Get(userId);
            _companyRepository.AddCompanyWorkerRequest(company, profile);
        }

        public void ApproveAdding(int userId, int adminId)
        {
            AdminUser admin = _userProfileRepository
                .Get(adminId)
                .EnsureIsAdmin();

            UserProfile user = _userProfileRepository.Get(userId);

            _companyRepository.ApproveRequest(user, admin);
        }

        private CompanyInfoDto WrapToDto(Company company)
        {
            UserProfile[] workers = _companyRepository.ReadWorkers(company);
            return CompanyInfoDto.Create(company, workers);
        }
    }
}
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

        public CompanyService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public CompanyInfoDto[] Get()
        {
            return _companyRepository.Read().SelectToArray(WrapToDto);
        }

        public CompanyInfoDto Get(int id)
        {
            return _companyRepository.ReadById(id).To(WrapToDto);
        }

        private CompanyInfoDto WrapToDto(Company company)
        {
            UserProfile[] workers = _companyRepository.ReadMembers(company.Id);
            return CompanyInfoDto.Create(company, workers);
        }
    }
}
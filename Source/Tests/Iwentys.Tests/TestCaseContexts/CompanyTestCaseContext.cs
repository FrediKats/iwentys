using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Companies.Entities;
using Iwentys.Features.Companies.Models;

namespace Iwentys.Tests.TestCaseContexts
{
    public class CompanyTestCaseContext
    {
        private readonly TestCaseContext _context;

        public CompanyTestCaseContext(TestCaseContext context)
        {
            _context = context;
        }

        public CompanyInfoDto WithCompany()
        {
            var company = new Company();
            company = _context.CompanyService.Create(company).Result;
            return new CompanyInfoDto(company);
        }

        public AuthorizedUser WithCompanyWorker(CompanyInfoDto companyInfo)
        {
            AuthorizedUser userInfo = _context.AccountManagementTestCaseContext.WithUser();
            AuthorizedUser admin = _context.AccountManagementTestCaseContext.WithUser(true);

            _context.CompanyService.RequestAdding(companyInfo.Id, userInfo.Id).Wait();
            _context.CompanyService.ApproveAdding(admin, userInfo.Id).Wait();

            return userInfo;
        }
    }
}
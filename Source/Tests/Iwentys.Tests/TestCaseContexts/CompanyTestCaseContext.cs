using Bogus;
using Iwentys.Domain;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Models;

namespace Iwentys.Tests.TestCaseContexts
{
    public class CompanyTestCaseContext
    {
        private readonly TestCaseContext _context;

        public CompanyTestCaseContext(TestCaseContext context)
        {
            _context = context;
        }

        public CompanyInfoDto WithCompany(AuthorizedUser initiator)
        {
            CompanyCreateArguments createArguments = new CompanyCreateArguments
            {
                Name = new Faker().Lorem.Word()
            };

            var company = _context.CompanyService.Create(initiator, createArguments).Result;
            return company;
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
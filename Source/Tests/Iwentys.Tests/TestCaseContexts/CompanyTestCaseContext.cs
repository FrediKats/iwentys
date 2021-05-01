using Bogus;
using Iwentys.Common.Databases;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Extended;
using Iwentys.Domain.Extended.Models;
using Microsoft.EntityFrameworkCore;

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

            IwentysUser creator = _context.UnitOfWork.GetRepository<IwentysUser>().GetById(initiator.Id).Result;

            var company = Company.Create(creator, createArguments);

            _context.UnitOfWork.GetRepository<Company>().Insert(company);
            _context.UnitOfWork.CommitAsync().Wait();

            return new CompanyInfoDto(company);
        }

        public AuthorizedUser WithCompanyWorker(CompanyInfoDto companyInfo)
        {
            AuthorizedUser userInfo = _context.AccountManagementTestCaseContext.WithUser();
            AuthorizedUser admin = _context.AccountManagementTestCaseContext.WithUser(true);
            IwentysUser iwentysUserUser = _context.UnitOfWork.GetRepository<IwentysUser>().GetById(admin.Id).Result;
            IwentysUser newMemberProfile = _context.UnitOfWork.GetRepository<IwentysUser>().FindByIdAsync(userInfo.Id).Result;

            Company company = _context.UnitOfWork.GetRepository<Company>().GetById(companyInfo.Id).Result;
            var newRequest = CompanyWorker.NewRequest(company, newMemberProfile, null);
            newRequest.Approve(iwentysUserUser);

            _context.UnitOfWork.GetRepository<CompanyWorker>().Insert(newRequest);
            _context.UnitOfWork.CommitAsync().Wait();

            return userInfo;
        }
    }
}
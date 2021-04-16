using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Domain;
using Iwentys.Domain.Models;
using Iwentys.Tests.TestCaseContexts;
using NUnit.Framework;

namespace Iwentys.Tests.Features
{
    public class CompanyFeatureTest
    {
        [Test]
        public async Task CreateCompanyWithWorker_ShouldReturnOneWorker()
        {
            TestCaseContext testCase = TestCaseContext.Case();
            AuthorizedUser admin = testCase.AccountManagementTestCaseContext.WithUser(true);
            CompanyInfoDto company = testCase.CompanyTestCaseContext.WithCompany(admin);
            AuthorizedUser user = testCase.CompanyTestCaseContext.WithCompanyWorker(company);

            List<IwentysUserInfoDto> companyMembers = (await testCase.CompanyService.Get(company.Id)).Workers;

            Assert.IsTrue(companyMembers.Any(cw => cw.Id == user.Id));
        }

        [Test]
        public async Task SendCompanyWorkerRequest_RequestWillExists()
        {
            TestCaseContext testCase = TestCaseContext.Case();
            AuthorizedUser admin = testCase.AccountManagementTestCaseContext.WithUser(true);
            CompanyInfoDto company = testCase.CompanyTestCaseContext.WithCompany(admin);
            AuthorizedUser worker = testCase.AccountManagementTestCaseContext.WithUser();

            await testCase.CompanyService.RequestAdding(company.Id, worker.Id);
            List<CompanyWorkRequestDto> companyRequests = await testCase.CompanyService.GetCompanyWorkRequest();
            CompanyInfoDto companyInfo = await testCase.CompanyService.Get(company.Id);

            List<IwentysUserInfoDto> companyMembers = companyInfo.Workers;

            Assert.IsFalse(companyMembers.Any(cw => cw.Id == worker.Id));
            Assert.IsTrue(companyRequests.Any(cr => cr.Worker.Id == worker.Id));
        }
    }
}
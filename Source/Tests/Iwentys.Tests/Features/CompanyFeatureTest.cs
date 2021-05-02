using System.Collections.Generic;
using System.Linq;
using Iwentys.Database.Seeding.FakerEntities;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Extended;
using Iwentys.Domain.Extended.Enums;
using Iwentys.Tests.TestCaseContexts;
using NUnit.Framework;

namespace Iwentys.Tests.Features
{
    public class CompanyFeatureTest
    {
        [Test]
        public void CreateCompanyWithWorker_ShouldReturnOneWorker()
        {
            TestCaseContext testCase = TestCaseContext.Case();
            AuthorizedUser admin = testCase.AccountManagementTestCaseContext.WithUser(true);
            AuthorizedUser newWorker = testCase.AccountManagementTestCaseContext.WithUser();
            Company company = testCase.CompanyTestCaseContext.WithCompany(admin);

            AuthorizedUser user = testCase.CompanyTestCaseContext.WithCompanyWorker(company, newWorker);

            Assert.IsTrue(company.Workers.Any(cw => cw.WorkerId == user.Id));
        }

        [Test]
        public void SendCompanyWorkerRequest_RequestWillExists()
        {
            TestCaseContext testCase = TestCaseContext.Case();
            AuthorizedUser admin = testCase.AccountManagementTestCaseContext.WithUser(true);
            Company company = testCase.CompanyTestCaseContext.WithCompany(admin);
            var profile = IwentysUser.Create(UsersFaker.Instance.IwentysUsers.Generate());

            
            var newRequest = company.NewRequest(profile, null);

            Assert.IsFalse(company.Workers.Any(cw => cw.WorkerId == profile.Id && cw.Type != CompanyWorkerType.Requested));
            Assert.IsTrue(company.Workers.Any(cw => cw.WorkerId == profile.Id && cw.Type == CompanyWorkerType.Requested));
        }
    }
}
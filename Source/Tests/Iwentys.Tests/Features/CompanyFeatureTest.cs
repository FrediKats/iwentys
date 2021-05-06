using System.Linq;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Companies;
using Iwentys.Infrastructure.DataAccess.Seeding.FakerEntities;
using Iwentys.Infrastructure.DataAccess.Seeding.FakerEntities.Extended;
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
            IwentysUser admin = testCase.AccountManagementTestCaseContext.WithIwentysUser(true);
            IwentysUser newWorker = testCase.AccountManagementTestCaseContext.WithIwentysUser();
            Company company = Company.Create(admin, CompanyFaker.Instance.NewCompany());

            CompanyWorker newRequest = company.NewRequest(newWorker, null);
            newRequest.Approve(admin);

            Assert.IsTrue(company.Workers.Any(cw => cw.Worker.Id == newWorker.Id));
        }

        [Test]
        public void SendCompanyWorkerRequest_RequestWillExists()
        {
            TestCaseContext testCase = TestCaseContext.Case();
            IwentysUser admin = testCase.AccountManagementTestCaseContext.WithIwentysUser(true);
            Company company = Company.Create(admin, CompanyFaker.Instance.NewCompany());
            var profile = IwentysUser.Create(UsersFaker.Instance.IwentysUsers.Generate());

            CompanyWorker newRequest = company.NewRequest(profile, null);

            Assert.IsFalse(company.Workers.Any(cw => cw.Worker.Id == profile.Id && cw.Type != CompanyWorkerType.Requested));
            Assert.IsTrue(company.Workers.Any(cw => cw.Worker.Id == profile.Id && cw.Type == CompanyWorkerType.Requested));
        }
    }
}
using System.Linq;
using Iwentys.Core.DomainModel;
using Iwentys.Models.Entities;
using Iwentys.Models.Transferable.Companies;
using Iwentys.Tests.Tools;
using NUnit.Framework;

namespace Iwentys.Tests.Core.Services
{
    public class CompanyServiceTest
    {
        [Test]
        public void CreateCompanyWithWorker_ShouldReturnOneWorker()
        {
            TestCaseContext testCase = TestCaseContext
                .Case()
                .WithCompany(out CompanyInfoResponse company)
                .WithCompanyWorker(company, out AuthorizedUser user);

            StudentEntity[] companyMembers = testCase.CompanyService.Get(company.Id).Workers;

            Assert.IsTrue(companyMembers.Length == 1);
            Assert.AreEqual(user.Id, companyMembers.Single().Id);
        }

        [Test]
        public void SendCompanyWorkerRequest_RequestWillExists()
        {
            TestCaseContext testCase = TestCaseContext
                .Case()
                .WithCompany(out CompanyInfoResponse company)
                .WithNewStudent(out AuthorizedUser worker);

            testCase.CompanyService.RequestAdding(company.Id, worker.Id);
            CompanyWorkRequestDto[] request = testCase.CompanyService.GetCompanyWorkRequest();
            StudentEntity[] companyMembers = testCase.CompanyService.Get(company.Id).Workers;

            Assert.IsTrue(companyMembers.Length == 0);
            Assert.IsTrue(request.Length == 1);
            Assert.AreEqual(worker.Id, request.Single().Worker.Id);
        }
    }
}
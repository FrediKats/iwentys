using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Features.Companies.Models;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;
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
                .WithCompany(out CompanyViewModel company)
                .WithCompanyWorker(company, out AuthorizedUser user);

            List<StudentEntity> companyMembers = testCase.CompanyService.Get(company.Id).Result.Workers;

            Assert.IsTrue(companyMembers.Count == 1);
            Assert.AreEqual(user.Id, companyMembers.Single().Id);
        }

        [Test]
        public async Task SendCompanyWorkerRequest_RequestWillExists()
        {
            TestCaseContext testCase = TestCaseContext
                .Case()
                .WithCompany(out CompanyViewModel company)
                .WithNewStudent(out AuthorizedUser worker);

            await testCase.CompanyService.RequestAdding(company.Id, worker.Id);
            List<CompanyWorkRequestDto> request = await testCase.CompanyService.GetCompanyWorkRequest();
            CompanyViewModel companyInfo = await testCase.CompanyService.Get(company.Id);

            List<StudentEntity> companyMembers = companyInfo.Workers;

            Assert.IsTrue(companyMembers.Count == 0);
            Assert.IsTrue(request.Count == 1);
            Assert.AreEqual(worker.Id, request.Single().Worker.Id);
        }
    }
}
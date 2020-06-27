using System.Linq;
using Iwentys.Models.Entities;
using Iwentys.Tests.Tools;
using NUnit.Framework;

namespace Iwentys.Tests.Core.Services
{
    public class CompanyServiceTest
    {
        [Test]
        public void CreateCompanyWithWorker_ShouldReturnOneWorker()
        {
            var testCase = TestCaseContext
                .Case()
                .WithCompany(out var company)
                .WithCompanyWorker(company, out var user);

            UserProfile[] companyMembers = testCase.CompanyService.Get(company.Id).Workers;

            Assert.IsTrue(companyMembers.Length == 1);
            Assert.AreEqual(user.Id, companyMembers.Single().Id);
        }
    }
}
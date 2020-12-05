﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Features.Companies.Models;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Models;
using Iwentys.Tests.Tools;
using NUnit.Framework;

namespace Iwentys.Tests.Features
{
    public class CompanyFeatureTest
    {
        [Test]
        public async Task CreateCompanyWithWorker_ShouldReturnOneWorker()
        {
            TestCaseContext testCase = TestCaseContext
                .Case()
                .WithCompany(out CompanyInfoDto company)
                .WithCompanyWorker(company, out AuthorizedUser user);

            List<StudentInfoDto> companyMembers = (await testCase.CompanyService.Get(company.Id)).Workers;

            Assert.IsTrue(companyMembers.Any(cw => cw.Id == user.Id));
        }

        [Test]
        public async Task SendCompanyWorkerRequest_RequestWillExists()
        {
            TestCaseContext testCase = TestCaseContext
                .Case()
                .WithCompany(out CompanyInfoDto company)
                .WithNewStudent(out AuthorizedUser worker);

            await testCase.CompanyService.RequestAdding(company.Id, worker.Id);
            List<CompanyWorkRequestDto> companyRequests = await testCase.CompanyService.GetCompanyWorkRequest();
            CompanyInfoDto companyInfo = await testCase.CompanyService.Get(company.Id);

            List<StudentInfoDto> companyMembers = companyInfo.Workers;

            Assert.IsFalse(companyMembers.Any(cw => cw.Id == worker.Id));
            Assert.IsTrue(companyRequests.Any(cr => cr.Worker.Id == worker.Id));
        }
    }
}
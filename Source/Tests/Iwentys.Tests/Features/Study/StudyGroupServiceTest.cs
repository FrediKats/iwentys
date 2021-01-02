using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Students.Enums;
using Iwentys.Features.Students.Models;
using Iwentys.Features.Study.Domain;
using Iwentys.Tests.TestCaseContexts;
using NUnit.Framework;

namespace Iwentys.Tests.Features.Study
{
    [TestFixture]
    public class StudyGroupServiceTest
    {
        [Test]
        public void ParseGroupName_EnsureCorrectValue()
        {
            const string groupAsString = "M3111";
            var groupName = new GroupName(groupAsString);

            Assert.AreEqual(1, groupName.Course);
            Assert.AreEqual(11, groupName.Number);
        }

        [Test]
        public async Task MakeGroupAdmin_EnsureUserIsAdmin()
        {
            TestCaseContext testCase = TestCaseContext
                .Case()
                .WithNewAdmin(out AuthorizedUser admin);

            List<StudentInfoDto> studentInfoDtos = await testCase
                .StudentService
                .GetAsync();
            StudentInfoDto newGroupAdmin = studentInfoDtos.First(s => s.Role == StudentRole.Common);

            await testCase.StudyGroupService.MakeGroupAdmin(admin, newGroupAdmin.Id);

            newGroupAdmin = await testCase.StudentService.GetAsync(newGroupAdmin.Id);
            Assert.AreEqual(StudentRole.GroupAdmin, newGroupAdmin.Role);
        }

        [Test]
        public async Task MakeGroupAdminWithoutPermission_NotEnoughPermissionException()
        {
            TestCaseContext testCase = TestCaseContext
                .Case()
                .WithNewStudent(out var commonUser);

            List<StudentInfoDto> studentInfoDtos = await testCase
                .StudentService
                .GetAsync();
            StudentInfoDto newGroupAdmin = studentInfoDtos.First(s => s.Role == StudentRole.Common);

            Assert.ThrowsAsync<InnerLogicException>(() => testCase.StudyGroupService.MakeGroupAdmin(commonUser, newGroupAdmin.Id));
        }
    }
}
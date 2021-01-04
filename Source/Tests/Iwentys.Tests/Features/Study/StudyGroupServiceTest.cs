using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Study.Domain;
using Iwentys.Features.Study.Models;
using Iwentys.Features.Study.Models.Students;
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

            //TODO: it's some kind of hack
            //TODO: implement creating group admin with group
            GroupProfileResponseDto studentGroup = await testCase.StudyGroupService.Get("M3101");

            StudentInfoDto newGroupAdmin = studentGroup.Students.First();


            //TODO: omg, we need to fetch group one more time coz Group admin id is not actual anymore
            await testCase.StudyGroupService.MakeGroupAdmin(admin, newGroupAdmin.Id);
            studentGroup = await testCase.StudyGroupService.Get("M3101");
            newGroupAdmin = await testCase.StudentService.Get(newGroupAdmin.Id);
            Assert.AreEqual(studentGroup.GroupAdmin.Id, newGroupAdmin.Id);
        }

        [Test]
        public async Task MakeGroupAdminWithoutPermission_NotEnoughPermissionException()
        {
            TestCaseContext testCase = TestCaseContext
                .Case()
                .WithNewStudent(out var commonUser);

            List<StudentInfoDto> studentInfoDtos = await testCase
                .StudentService
                .Get();
            StudentInfoDto newGroupAdmin = studentInfoDtos.First();

            Assert.ThrowsAsync<InnerLogicException>(() => testCase.StudyGroupService.MakeGroupAdmin(commonUser, newGroupAdmin.Id));
        }
    }
}
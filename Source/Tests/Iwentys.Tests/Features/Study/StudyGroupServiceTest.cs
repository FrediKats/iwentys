using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Iwentys.Domain;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Models;
using Iwentys.Domain.Study;
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
            TestCaseContext testCase = TestCaseContext.Case();
            AuthorizedUser admin = testCase.AccountManagementTestCaseContext.WithUser(true);
            GroupProfileResponseDto studyGroup = testCase.StudyTestCaseContext.WithStudyGroup();
            AuthorizedUser newGroupAdmin = testCase.StudyTestCaseContext.WithNewStudent(studyGroup);

            await testCase.StudyService.MakeGroupAdmin(admin, newGroupAdmin.Id);

            studyGroup = await testCase.StudyService.GetStudentStudyGroup(newGroupAdmin.Id);
            Assert.AreEqual(studyGroup.GroupAdmin.Id, newGroupAdmin.Id);
        }

        [Test]
        public async Task MakeGroupAdminWithoutPermission_NotEnoughPermissionException()
        {
            TestCaseContext testCase = TestCaseContext.Case();
            AuthorizedUser commonUser = testCase.AccountManagementTestCaseContext.WithUser();
            GroupProfileResponseDto studyGroup = testCase.StudyTestCaseContext.WithStudyGroup();
            AuthorizedUser newGroupAdmin = testCase.StudyTestCaseContext.WithNewStudent(studyGroup);

            Assert.ThrowsAsync<InnerLogicException>(() => testCase.StudyService.MakeGroupAdmin(commonUser, newGroupAdmin.Id));
        }
    }
}
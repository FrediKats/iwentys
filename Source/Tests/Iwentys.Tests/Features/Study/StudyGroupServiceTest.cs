using Iwentys.Common;
using Iwentys.DataAccess.Seeding;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.Tests.TestCaseContexts;
using NUnit.Framework;

namespace Iwentys.Tests.Features.Study;

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
    public void MakeGroupAdmin_EnsureUserIsAdmin()
    {
        TestCaseContext testCase = TestCaseContext.Case();
        IwentysUser admin = testCase.AccountManagementTestCaseContext.WithIwentysUser(true);
        StudyGroup studyGroup = StudyGroupFaker.Instance.CreateGroup();

        Student newGroupAdmin = testCase.StudyTestCaseContext.WithNewStudentAsStudent(studyGroup);
        studyGroup.MakeAdmin(admin, newGroupAdmin);

        Assert.AreEqual(studyGroup.GroupAdminId, newGroupAdmin.Id);
    }

    [Test]
    public void MakeGroupAdminWithoutPermission_NotEnoughPermissionException()
    {
        TestCaseContext testCase = TestCaseContext.Case();
        IwentysUser commonUser = testCase.AccountManagementTestCaseContext.WithIwentysUser();
        StudyGroup studyGroup = StudyGroupFaker.Instance.CreateGroup();
        Student newGroupAdmin = testCase.StudyTestCaseContext.WithNewStudentAsStudent(studyGroup);

        Assert.Throws<InnerLogicException>(() => studyGroup.MakeAdmin(commonUser, newGroupAdmin));
    }
}
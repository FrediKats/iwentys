using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Enums;
using Iwentys.Domain.SubjectAssignments;
using Iwentys.Domain.SubjectAssignments.Models;
using Iwentys.Infrastructure.DataAccess.Seeding.FakerEntities.Study;
using Iwentys.Tests.TestCaseContexts;
using NUnit.Framework;

namespace Iwentys.Tests.Modules.SubjectAssignments
{
    [TestFixture]
    public class SubjectAssignmentTest
    {
        [Test]
        public void ParseGroupName_EnsureCorrectValue()
        {
            TestCaseContext testCase = TestCaseContext.Case();

            IwentysUser admin = testCase.AccountManagementTestCaseContext.WithIwentysUser(true);
            Subject subject = SubjectFaker.Instance.Generate();
            StudyGroup studyGroup = StudyGroupFaker.Instance.CreateGroup();
            SubjectAssignmentCreateArguments arguments = SubjectAssignmentFaker.Instance.CreateSubjectAssignmentCreateArguments(subject.Id);

            GroupSubject groupSubject = subject.AddGroup(studyGroup, StudySemesterExtensions.GetDefault(), admin, admin);
            var subjectAssignment = SubjectAssignment.Create(admin, subject, arguments);
            subjectAssignment.AddAssignmentForGroup(admin, studyGroup);

            Assert.AreEqual(1, subjectAssignment.GroupSubjectAssignments.Count);
        }
    }
}
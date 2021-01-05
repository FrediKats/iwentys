using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Models;
using Iwentys.Tests.TestCaseContexts;
using NUnit.Framework;

namespace Iwentys.Tests.Features.Study
{
    [TestFixture]
    public class SubjectAssignmentServiceTest
    {
        [Test]
        public void ParseGroupName_EnsureCorrectValue()
        {
            TestCaseContext testCaseContext = TestCaseContext.Case();
            AuthorizedUser admin = testCaseContext.AccountManagementTestCaseContext.WithUser(true);
            GroupProfileResponseDto studyGroup = testCaseContext.StudyTestCaseContext.WithStudyGroup();
            Subject subject = testCaseContext.StudyTestCaseContext.WithSubject();
            GroupSubject groupSubject = testCaseContext.StudyTestCaseContext.WithGroupSubject(studyGroup, subject);

            testCaseContext.StudyTestCaseContext.WithSubjectAssignment(admin, groupSubject);
        }
    }
}
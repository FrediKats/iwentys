using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Models;
using Iwentys.Tests.TestCaseContexts;
using NUnit.Framework;

namespace Iwentys.Tests.Features.Study
{
    [TestFixture]
    public class SubjectAssignmentServiceTest
    {
        [Test]
        public void CreateSubjectAssignment_Ok()
        {
            TestCaseContext testCaseContext = TestCaseContext.Case();
            AuthorizedUser admin = testCaseContext.AccountManagementTestCaseContext.WithUser(true);
            GroupProfileResponseDto studyGroup = testCaseContext.StudyTestCaseContext.WithStudyGroup();
            Subject subject = testCaseContext.StudyTestCaseContext.WithSubject();
            GroupSubject groupSubject = testCaseContext.StudyTestCaseContext.WithGroupSubject(studyGroup, subject, admin);

            SubjectAssignmentDto subjectAssignment = testCaseContext.StudyTestCaseContext.WithSubjectAssignment(admin, groupSubject);
        }

        [Test]
        public void CreateSubjectAssignmentSubmit_SubmitShouldExists()
        {
            TestCaseContext testCaseContext = TestCaseContext.Case();
            AuthorizedUser admin = testCaseContext.AccountManagementTestCaseContext.WithUser(true);
            GroupProfileResponseDto studyGroup = testCaseContext.StudyTestCaseContext.WithStudyGroup();
            AuthorizedUser student = testCaseContext.StudyTestCaseContext.WithNewStudent(studyGroup);
            Subject subject = testCaseContext.StudyTestCaseContext.WithSubject();
            GroupSubject groupSubject = testCaseContext.StudyTestCaseContext.WithGroupSubject(studyGroup, subject, admin);
            SubjectAssignmentDto subjectAssignment = testCaseContext.StudyTestCaseContext.WithSubjectAssignment(admin, groupSubject);

            SubjectAssignmentSubmitDto subjectAssignmentSubmit = testCaseContext.StudyTestCaseContext.WithSubjectAssignmentSubmit(student, subjectAssignment);
        }

        [Test]
        public void SendSubjectAssignmentSubmitFeedback_StateShouldChange()
        {
            TestCaseContext testCaseContext = TestCaseContext.Case();
            AuthorizedUser admin = testCaseContext.AccountManagementTestCaseContext.WithUser(true);
            GroupProfileResponseDto studyGroup = testCaseContext.StudyTestCaseContext.WithStudyGroup();
            AuthorizedUser student = testCaseContext.StudyTestCaseContext.WithNewStudent(studyGroup);
            Subject subject = testCaseContext.StudyTestCaseContext.WithSubject();
            GroupSubject groupSubject = testCaseContext.StudyTestCaseContext.WithGroupSubject(studyGroup, subject, admin);
            SubjectAssignmentDto subjectAssignment = testCaseContext.StudyTestCaseContext.WithSubjectAssignment(admin, groupSubject);
            SubjectAssignmentSubmitDto subjectAssignmentSubmit = testCaseContext.StudyTestCaseContext.WithSubjectAssignmentSubmit(student, subjectAssignment);

            testCaseContext.StudyTestCaseContext.WithSubjectAssignmentSubmitFeedback(admin, subjectAssignmentSubmit);
        }
    }
}
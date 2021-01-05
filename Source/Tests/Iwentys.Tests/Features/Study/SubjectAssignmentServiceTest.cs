using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Models;
using Iwentys.Features.Study.SubjectAssignments.Models;
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
            GroupSubject groupSubject = testCaseContext.StudyTestCaseContext.WithGroupSubject(studyGroup, subject);

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
            GroupSubject groupSubject = testCaseContext.StudyTestCaseContext.WithGroupSubject(studyGroup, subject);
            SubjectAssignmentDto subjectAssignment = testCaseContext.StudyTestCaseContext.WithSubjectAssignment(admin, groupSubject);

            SubjectAssignmentSubmitDto subjectAssignmentSubmit = testCaseContext.StudyTestCaseContext.WithSubjectAssignmentSubmit(student, subjectAssignment);
        }

        [Test]
        public void SendSubjectAssignemntSubmitFeedback_StateShouldChange()
        {
            TestCaseContext testCaseContext = TestCaseContext.Case();
            AuthorizedUser admin = testCaseContext.AccountManagementTestCaseContext.WithUser(true);
            GroupProfileResponseDto studyGroup = testCaseContext.StudyTestCaseContext.WithStudyGroup();
            AuthorizedUser student = testCaseContext.StudyTestCaseContext.WithNewStudent(studyGroup);
            Subject subject = testCaseContext.StudyTestCaseContext.WithSubject();
            GroupSubject groupSubject = testCaseContext.StudyTestCaseContext.WithGroupSubject(studyGroup, subject);
            SubjectAssignmentDto subjectAssignment = testCaseContext.StudyTestCaseContext.WithSubjectAssignment(admin, groupSubject);
            SubjectAssignmentSubmitDto subjectAssignmentSubmit = testCaseContext.StudyTestCaseContext.WithSubjectAssignmentSubmit(student, subjectAssignment);

            testCaseContext.StudyTestCaseContext.WithSubjectAssignmentSubmitFeedback(admin, subjectAssignmentSubmit);
        }
    }
}
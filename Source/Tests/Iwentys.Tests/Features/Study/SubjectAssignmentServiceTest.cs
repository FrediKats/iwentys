using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Models;
using Iwentys.Domain.SubjectAssignments;
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
            IwentysUser admin = testCaseContext.AccountManagementTestCaseContext.WithIwentysUser(true);
            GroupProfileResponseDto studyGroup = testCaseContext.StudyTestCaseContext.WithStudyGroup();
            Subject subject = testCaseContext.StudyTestCaseContext.WithSubject();
            GroupSubject groupSubject = testCaseContext.StudyTestCaseContext.WithGroupSubject(studyGroup, subject, admin);

            SubjectAssignment subjectAssignment = testCaseContext.StudyTestCaseContext.WithSubjectAssignment(admin, groupSubject);
        }

        [Test]
        public void CreateSubjectAssignmentSubmit_SubmitShouldExists()
        {
            TestCaseContext testCaseContext = TestCaseContext.Case();
            IwentysUser admin = testCaseContext.AccountManagementTestCaseContext.WithIwentysUser(true);
            GroupProfileResponseDto studyGroup = testCaseContext.StudyTestCaseContext.WithStudyGroup();
            Student student = testCaseContext.StudyTestCaseContext.WithNewStudentAsStudent(studyGroup);
            Subject subject = testCaseContext.StudyTestCaseContext.WithSubject();
            GroupSubject groupSubject = testCaseContext.StudyTestCaseContext.WithGroupSubject(studyGroup, subject, admin);
            SubjectAssignment subjectAssignment = testCaseContext.StudyTestCaseContext.WithSubjectAssignment(admin, groupSubject);

            SubjectAssignmentSubmit subjectAssignmentSubmit = testCaseContext.StudyTestCaseContext.WithSubjectAssignmentSubmit(student, subjectAssignment);
        }

        [Test]
        public void SendSubjectAssignmentSubmitFeedback_StateShouldChange()
        {
            TestCaseContext testCaseContext = TestCaseContext.Case();
            IwentysUser admin = testCaseContext.AccountManagementTestCaseContext.WithIwentysUser(true);
            GroupProfileResponseDto studyGroup = testCaseContext.StudyTestCaseContext.WithStudyGroup();
            Student student = testCaseContext.StudyTestCaseContext.WithNewStudentAsStudent(studyGroup);
            Subject subject = testCaseContext.StudyTestCaseContext.WithSubject();
            GroupSubject groupSubject = testCaseContext.StudyTestCaseContext.WithGroupSubject(studyGroup, subject, admin);
            SubjectAssignment subjectAssignment = testCaseContext.StudyTestCaseContext.WithSubjectAssignment(admin, groupSubject);
            SubjectAssignmentSubmit subjectAssignmentSubmit = testCaseContext.StudyTestCaseContext.WithSubjectAssignmentSubmit(student, subjectAssignment);

            testCaseContext.StudyTestCaseContext.WithSubjectAssignmentSubmitFeedback(admin, subjectAssignmentSubmit);
        }
    }
}
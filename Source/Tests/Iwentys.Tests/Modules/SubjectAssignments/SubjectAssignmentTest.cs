using Iwentys.DataAccess.Seeding;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.Domain.SubjectAssignments;
using Iwentys.Tests.TestCaseContexts;
using NUnit.Framework;

namespace Iwentys.Tests.Modules.SubjectAssignments;

[TestFixture]
public class SubjectAssignmentTest
{
    [Test]
    public void CreateSubjectAssignment_Ok()
    {
        TestCaseContext testCase = TestCaseContext.Case();

        IwentysUser admin = testCase.AccountManagementTestCaseContext.WithIwentysUser(true);
        Subject subject = SubjectFaker.Instance.Generate();
        StudyGroup studyGroup = StudyGroupFaker.Instance.CreateGroup();
        SubjectAssignmentCreateArguments arguments = SubjectAssignmentFaker.Instance.CreateSubjectAssignmentCreateArguments(subject.Id);

        StudySemester studySemester = StudySemesterExtensions.GetDefault();
        var groupSubject1 = new GroupSubject(subject.Id, studyGroup.Id, studySemester);
        GroupSubject groupSubject = groupSubject1;
        var subjectAssignment = SubjectAssignment.Create(admin, subject.Id, arguments);
        GroupSubjectAssignment groupSubjectAssignment = subjectAssignment.AddAssignmentForGroup(admin, groupSubject.StudyGroupId);

        Assert.AreEqual(1, subjectAssignment.GroupSubjectAssignments.Count);
    }

    [Test]
    public void CreateSubjectAssignmentSubmit_Ok()
    {
        TestCaseContext testCase = TestCaseContext.Case();

        IwentysUser admin = testCase.AccountManagementTestCaseContext.WithIwentysUser(true);
        Subject subject = SubjectFaker.Instance.Generate();
        StudyGroup studyGroup = StudyGroupFaker.Instance.CreateGroup();
        SubjectAssignmentCreateArguments arguments = SubjectAssignmentFaker.Instance.CreateSubjectAssignmentCreateArguments(subject.Id);

        StudySemester studySemester = StudySemesterExtensions.GetDefault();
        var groupSubject1 = new GroupSubject(subject.Id, studyGroup.Id, studySemester);
        GroupSubject groupSubject = groupSubject1;
        var subjectAssignment = SubjectAssignment.Create(admin, subject.Id, arguments);
        GroupSubjectAssignment groupSubjectAssignment = subjectAssignment.AddAssignmentForGroup(admin, groupSubject.StudyGroupId);


        var student = Student.Create(UsersFaker.Instance.Students.Generate());
        studyGroup.AddStudent(student);
        groupSubjectAssignment.CreateSubmit(student, SubjectAssignmentFaker.Instance.CreateSubjectAssignmentSubmitCreateArguments(subjectAssignment.Id));

        Assert.AreEqual(1, groupSubjectAssignment.SubjectAssignmentSubmits.Count);
    }

    [Test]
    public void ApproveSubjectAssignmentSubmit_Ok()
    {
        TestCaseContext testCase = TestCaseContext.Case();

        IwentysUser admin = testCase.AccountManagementTestCaseContext.WithIwentysUser(true);
        Subject subject = SubjectFaker.Instance.Generate();
        StudyGroup studyGroup = StudyGroupFaker.Instance.CreateGroup();
        SubjectAssignmentCreateArguments arguments = SubjectAssignmentFaker.Instance.CreateSubjectAssignmentCreateArguments(subject.Id);

        StudySemester studySemester = StudySemesterExtensions.GetDefault();
        var groupSubject1 = new GroupSubject(subject.Id, studyGroup.Id, studySemester);
        GroupSubject groupSubject = groupSubject1;
        var subjectAssignment = SubjectAssignment.Create(admin, subject.Id, arguments);
        GroupSubjectAssignment groupSubjectAssignment = subjectAssignment.AddAssignmentForGroup(admin, groupSubject.StudyGroupId);

        var student = Student.Create(UsersFaker.Instance.Students.Generate());
        studyGroup.AddStudent(student);
        SubjectAssignmentSubmit subjectAssignmentSubmit = groupSubjectAssignment.CreateSubmit(student, SubjectAssignmentFaker.Instance.CreateSubjectAssignmentSubmitCreateArguments(subjectAssignment.Id));

        subjectAssignmentSubmit.AddFeedback(admin, SubjectAssignmentFaker.Instance.CreateFeedback(subjectAssignmentSubmit.Id, FeedbackType.Approve));

        Assert.AreEqual(SubmitState.Approved, subjectAssignmentSubmit.State);
    }
}
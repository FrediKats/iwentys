﻿using System.Collections.Generic;
using System.Linq;
using Iwentys.DataAccess.Seeding;
using Iwentys.Domain.Assignments;
using Iwentys.Domain.Study;
using Iwentys.Tests.TestCaseContexts;
using NUnit.Framework;

namespace Iwentys.Tests.Features;

[TestFixture]
public class AssignmentServiceTest
{
    [Test]
    public void CreateAssignment_Ok()
    {
        TestCaseContext testCase = TestCaseContext.Case();
        StudyGroup studyGroup = StudyGroupFaker.Instance.CreateGroup();
        Student student = testCase.StudyTestCaseContext.WithNewStudentAsStudent(studyGroup);

        List<StudentAssignment> assignments = StudentAssignment.Create(student, AssignmentFaker.Instance.CreateAssignmentCreateArguments(), studyGroup.Students);

        Assert.IsNotNull(assignments.Any());
    }

    [Test]
    public void CompleteAssignment_StateShouldChanged()
    {
        TestCaseContext testCase = TestCaseContext.Case();
        StudyGroup studyGroup = StudyGroupFaker.Instance.CreateGroup();
        Student student = testCase.StudyTestCaseContext.WithNewStudentAsStudent(studyGroup);

        List<StudentAssignment> assignments = StudentAssignment.Create(student, AssignmentFaker.Instance.CreateAssignmentCreateArguments(), studyGroup.Students);
        StudentAssignment studentAssignment = assignments.First();
        studentAssignment.MarkCompleted();

        Assert.IsTrue(studentAssignment.IsCompleted);
    }
}
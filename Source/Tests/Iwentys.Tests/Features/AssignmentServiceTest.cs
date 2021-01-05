using System.Threading.Tasks;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Assignments.Models;
using Iwentys.Features.Study.Models;
using Iwentys.Tests.TestCaseContexts;
using NUnit.Framework;

namespace Iwentys.Tests.Features
{
    [TestFixture]
    public class AssignmentServiceTest
    {
        [Test]
        public async Task CreateAssignment_Ok()
        {
            TestCaseContext testCase = TestCaseContext.Case();
            GroupProfileResponseDto studyGroup = testCase.StudyTestCaseContext.WithStudyGroup();
            AuthorizedUser student = testCase.StudyTestCaseContext.WithNewStudent(studyGroup);
            
            AssignmentInfoDto assignmentInfoDto = testCase.AssignmentTestCaseContext.WithAssignment(student);

            Assert.IsNotNull(assignmentInfoDto);
        }

        [Test]
        public async Task CompleteAssignment_StateShouldChanged()
        {
            TestCaseContext testCase = TestCaseContext.Case();
            GroupProfileResponseDto studyGroup = testCase.StudyTestCaseContext.WithStudyGroup();
            AuthorizedUser student = testCase.StudyTestCaseContext.WithNewStudent(studyGroup);
            AssignmentInfoDto assignmentInfoDto = testCase.AssignmentTestCaseContext.WithAssignment(student);

            await testCase.AssignmentService.Complete(student, assignmentInfoDto.Id);

            assignmentInfoDto = await testCase.AssignmentService.GetStudentAssignment(student, assignmentInfoDto.Id);
            Assert.IsTrue(assignmentInfoDto.IsCompeted);
        }
    }
}
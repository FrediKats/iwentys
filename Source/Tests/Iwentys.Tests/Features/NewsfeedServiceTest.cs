using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Study.Models;
using Iwentys.Tests.TestCaseContexts;
using NUnit.Framework;

namespace Iwentys.Tests.Features
{
    [TestFixture]
    public class NewsfeedServiceTest
    {
        [Test]
        public void CreateSubjectNews_Ok()
        {
            //GroupProfileResponseDto studyGroup = testCase.StudyTestCaseContext.WithStudyGroup();
            //AuthorizedUser student = testCase.StudyTestCaseContext.WithNewStudent(studyGroup);

            TestCaseContext testCase = TestCaseContext.Case();
            AuthorizedUser admin = testCase.AccountManagementTestCaseContext.WithUser(true);
            SubjectProfileDto subject = testCase.NewsfeedTestCaseContext.WithSubject();
            
            testCase.NewsfeedTestCaseContext.WithSubjectNews(subject, admin);
        }
    }
}
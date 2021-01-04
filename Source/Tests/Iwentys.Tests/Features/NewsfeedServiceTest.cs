using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Study.Entities;
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
            TestCaseContext testCase = TestCaseContext.Case();
            GroupProfileResponseDto studyGroup = testCase.StudyTestCaseContext.WithStudyGroup();
            AuthorizedUser student = testCase.StudyTestCaseContext.WithNewStudent(studyGroup);
            AuthorizedUser admin = testCase.AccountManagementTestCaseContext.WithUser(true);

            testCase
                .WithSubject(out var subject)
                .WithSubjectNews(subject, admin);
        }
    }
}
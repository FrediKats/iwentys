using Iwentys.Features.Students.Enums;
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
            TestCaseContext
                .Case()
                .WithNewStudent(out var user, UserType.Admin)
                .WithSubject(out var subject)
                .WithSubjectNews(subject, user);
        }
    }
}
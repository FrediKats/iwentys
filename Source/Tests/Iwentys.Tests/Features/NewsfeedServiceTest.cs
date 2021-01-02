using Iwentys.Features.AccountManagement.Domain;
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
                .WithNewAdmin(out AuthorizedUser user)
                .WithSubject(out var subject)
                .WithSubjectNews(subject, user);
        }
    }
}
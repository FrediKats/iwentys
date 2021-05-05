using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Karmas;
using Iwentys.Tests.TestCaseContexts;
using NUnit.Framework;

namespace Iwentys.Tests.Features.Gamification
{
    [TestFixture]
    public class KarmaServiceTest
    {
        [Test]
        public void AddKarma_ShouldContainUpVote()
        {
            TestCaseContext testCase = TestCaseContext.Case();
            IwentysUser first = testCase.AccountManagementTestCaseContext.WithIwentysUser();
            IwentysUser second = testCase.AccountManagementTestCaseContext.WithIwentysUser();

            var karmaUpVote = KarmaUpVote.Create(first, second);

            Assert.IsTrue(karmaUpVote.AuthorId == first.Id);
        }
    }
}
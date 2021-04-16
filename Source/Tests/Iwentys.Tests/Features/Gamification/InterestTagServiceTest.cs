using System.Collections.Generic;
using System.Linq;
using Iwentys.Domain;
using Iwentys.Domain.Models;
using Iwentys.Tests.TestCaseContexts;
using NUnit.Framework;

namespace Iwentys.Tests.Features.Gamification
{
    [TestFixture]
    public class InterestTagServiceTest
    {
        [Test]
        public void AddTagToUser_EnsureUserHaveTag()
        {
            TestCaseContext testCase = TestCaseContext.Case();
            AuthorizedUser user = testCase.AccountManagementTestCaseContext.WithUser(true);
            InterestTagDto tag = testCase.GamificationTestCaseContext.WithInterestTag();

            testCase.GamificationTestCaseContext.WithUserInterestTag(tag, user);

            List<InterestTagDto> tags = testCase.InterestTagService.GetUserTags(user.Id).Result;
            Assert.IsTrue(tags.Any(t => t.Id == tag.Id));
        }
    }
}
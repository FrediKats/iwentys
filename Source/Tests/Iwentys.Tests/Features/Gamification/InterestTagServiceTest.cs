using System.Collections.Generic;
using System.Linq;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.InterestTags.Models;
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
            var testCase = TestCaseContext.Case()
                .WithInterestTag(out var tag);
            AuthorizedUser user = testCase.AccountManagementTestCaseContext.WithUser(true);
            testCase.WithUserInterestTag(tag, user);

            List<InterestTagDto> interestTagDtos = testCase.InterestTagService.GetUserTags(user.Id).Result;
            Assert.IsTrue(interestTagDtos.Any(t => t.Id == tag.Id));
        }
    }
}
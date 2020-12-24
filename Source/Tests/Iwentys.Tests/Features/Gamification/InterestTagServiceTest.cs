using System.Collections.Generic;
using System.Linq;
using Iwentys.Features.Gamification.Models;
using Iwentys.Features.Students.Enums;
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
            var testCase = TestCaseContext
                .Case()
                .WithNewStudent(out var user, StudentRole.Admin)
                .WithInterestTag(out var tag)
                .WithUserInterestTag(tag, user);

            List<InterestTagDto> interestTagDtos = testCase.InterestTagService.GetStudentTags(user.Id).Result;
            Assert.IsTrue(interestTagDtos.Any(t => t.Id == tag.Id));
        }
    }
}
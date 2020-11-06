using System.Linq;
using Iwentys.Core.DomainModel;
using Iwentys.Features.Achievements;
using Iwentys.Models.Entities.Gamification;
using Iwentys.Tests.Tools;
using NUnit.Framework;

namespace Iwentys.Tests.Core.Gamification
{
    [TestFixture]
    public class AchievementTest
    {
        [Test]
        public void CreateGuild_ShouldReturnCreatorAsMember()
        {
            TestCaseContext testCase = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user);

            testCase.StudentService.AddGithubUsernameAsync(user.Id, "username");
            StudentAchievementEntity achievement = testCase.Context.StudentAchievements.FirstOrDefault(a => a.StudentId == user.Id && a.AchievementId == AchievementList.AddGithubAchievement.Id);
            Assert.NotNull(achievement);
        }
    }
}
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Features.Achievements;
using Iwentys.Features.StudentFeature;
using Iwentys.Models.Entities.Gamification;
using Iwentys.Tests.Tools;
using NUnit.Framework;

namespace Iwentys.Tests.Core.Gamification
{
    [TestFixture]
    public class AchievementTest
    {
        [Test]
        public async Task CreateGuild_ShouldReturnCreatorAsMember()
        {
            TestCaseContext testCase = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user);

            await testCase.StudentService.AddGithubUsernameAsync(user.Id, "username");
            StudentAchievementEntity achievement = testCase.Context
                .StudentAchievements
                .FirstOrDefault(a => a.StudentId == user.Id && a.AchievementId == AchievementList.AddGithubAchievement.Id);
            
            Assert.NotNull(achievement);
        }
    }
}
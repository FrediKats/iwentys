using System.Linq;
using System.Threading.Tasks;
using Iwentys.Features.Achievements.Domain;
using Iwentys.Features.Achievements.Entities;
using Iwentys.Features.Students.Domain;
using Iwentys.Tests.Tools;
using NUnit.Framework;

namespace Iwentys.Tests.Features
{
    [TestFixture]
    public class AchievementFeatureTest
    {
        //TODO:
        [Test]
        [Ignore("Achievement disabled")]
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
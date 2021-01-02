using System.Linq;
using System.Threading.Tasks;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Achievements.Domain;
using Iwentys.Features.Achievements.Models;
using Iwentys.Tests.TestCaseContexts;
using NUnit.Framework;

namespace Iwentys.Tests.Features
{
    [TestFixture]
    public class AchievementFeatureTest
    {
        [Test]
        public async Task CreateGuild_ShouldReturnCreatorAsMember()
        {
            TestCaseContext testCase = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user);

            await testCase.StudentService.AddGithubUsernameAsync(user.Id, "username");
            AchievementDto studentAchievementEntity = (testCase
                    .AchievementService
                    .GetForStudent(user.Id))
                .FirstOrDefault(a => a.Id == AchievementList.AddGithubAchievement.Id);


            Assert.NotNull(studentAchievementEntity);
        }
    }
}
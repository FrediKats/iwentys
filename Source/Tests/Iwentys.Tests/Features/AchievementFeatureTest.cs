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
        [Ignore("Need to move github update to iwentys user service")]
        public async Task CreateGuild_ShouldReturnCreatorAsMember()
        {
            TestCaseContext testCase = TestCaseContext.Case();
            AuthorizedUser user = testCase.AccountManagementTestCaseContext.WithUser();

            await testCase.StudentService.AddGithubUsername(user.Id, "username");
            AchievementInfoDto studentAchievementEntity = (await testCase
                    .AchievementService
                    .GetForStudent(user.Id))
                .FirstOrDefault(a => a.Id == AchievementList.AddGithubAchievement.Id);


            Assert.NotNull(studentAchievementEntity);
        }
    }
}
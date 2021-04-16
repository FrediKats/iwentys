using System.Linq;
using System.Threading.Tasks;
using Iwentys.Domain;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.Models;
using Iwentys.Tests.TestCaseContexts;
using NUnit.Framework;

namespace Iwentys.Tests.Features
{
    [TestFixture]
    public class AchievementFeatureTest
    {
        [Test]
        [Ignore("Need to implement eventing system")]
        public async Task CreateGuild_ShouldReturnCreatorAsMember()
        {
            TestCaseContext testCase = TestCaseContext.Case();
            GroupProfileResponseDto studyGroup = testCase.StudyTestCaseContext.WithStudyGroup();
            AuthorizedUser user = testCase.StudyTestCaseContext.WithNewStudent(studyGroup);

            await testCase.IwentysUserService.AddGithubUsername(user.Id, "username");
            AchievementInfoDto studentAchievementEntity = (await testCase
                    .AchievementService
                    .GetForStudent(user.Id))
                .FirstOrDefault(a => a.Id == AchievementList.AddGithubAchievement.Id);


            Assert.NotNull(studentAchievementEntity);
        }
    }
}
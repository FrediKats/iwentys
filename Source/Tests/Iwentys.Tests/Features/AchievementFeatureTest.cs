using System.Threading.Tasks;
using Iwentys.Features.Students.Domain;
using Iwentys.Tests.TestCaseContexts;
using NUnit.Framework;

namespace Iwentys.Tests.Features
{
    [TestFixture]
    public class AchievementFeatureTest
    {
        //TODO: fix achievements
        [Test]
        [Ignore("Achievement disabled")]
        public async Task CreateGuild_ShouldReturnCreatorAsMember()
        {
            TestCaseContext testCase = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user);

            await testCase.StudentService.AddGithubUsernameAsync(user.Id, "username");
            //StudentAchievementEntity studentAchievementEntity = testCase.AchievementRepository
            //    .ReadStudentAchievements()
            //    .FirstOrDefault(a => a.StudentId == user.Id && a.AchievementId == AchievementList.AddGithubAchievement.Id);
            
            
            //Assert.NotNull(studentAchievementEntity);
        }
    }
}
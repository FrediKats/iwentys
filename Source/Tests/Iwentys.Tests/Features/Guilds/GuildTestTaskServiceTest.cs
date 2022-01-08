using System.Linq;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.PeerReview;
using Iwentys.Infrastructure.DataAccess.Seeding.FakerEntities.Guilds;
using Iwentys.Tests.TestCaseContexts;
using NUnit.Framework;

namespace Iwentys.Tests.Features.Guilds
{
    [TestFixture]
    public class GuildTestTaskServiceTest
    {
        [Test]
        public void AcceptTestTask_ShouldBeInList()
        {
            TestCaseContext context = TestCaseContext.Case();
            IwentysUser user = context.AccountManagementTestCaseContext.WithIwentysUser(true);
            IwentysUser newMember = context.AccountManagementTestCaseContext.WithIwentysUser();
            var guild = Guild.Create(user, null, GuildFaker.Instance.GetGuildCreateArguments());
            guild.Members.Add(new GuildMember(guild, newMember, GuildMemberType.Member));

            var guildTestTaskSolution = GuildTestTaskSolution.Create(guild, newMember);

            Assert.IsTrue(guildTestTaskSolution.AuthorId == newMember.Id);
        }

        [Test]
        public void SubmitTestTask_StateShouldBeSubmitted()
        {
            TestCaseContext context = TestCaseContext.Case();
            IwentysUser user = context.AccountManagementTestCaseContext.WithIwentysUser();
            IwentysUser guildNewcomer = context.AccountManagementTestCaseContext.WithIwentysUser();
            var guild = Guild.Create(user, null, GuildFaker.Instance.GetGuildCreateArguments());
            guild.Members.Add(new GuildMember(guild, guildNewcomer, GuildMemberType.Member));

            var newGithubUser = new GithubUser
            {
                IwentysUserId = guildNewcomer.Id,
                Username = guildNewcomer.GithubUsername
            };

            GithubUser githubUser = newGithubUser;
            GithubProject githubProject = context.GithubTestCaseContext.WithStudentProject(guildNewcomer, githubUser);
            var testTaskSolution = GuildTestTaskSolution.Create(guild, guildNewcomer);

            ProjectReviewRequest reviewRequest = ProjectReviewRequest.CreateGuildReviewRequest(guildNewcomer, githubProject, testTaskSolution, guild);

            GuildTestTaskSolution userResponse = guild.TestTasks.First(t => t.AuthorId == guildNewcomer.Id);

            Assert.AreEqual(GuildTestTaskState.Submitted, userResponse.GetState());
        }

        [Test]
        public void CompleteTestTask_StateShouldBeCompleted()
        {
            TestCaseContext context = TestCaseContext.Case();
            IwentysUser user = context.AccountManagementTestCaseContext.WithIwentysUser();
            IwentysUser guildNewcomer = context.AccountManagementTestCaseContext.WithIwentysUser();
            var guild = Guild.Create(user, null, GuildFaker.Instance.GetGuildCreateArguments());
            guild.Members.Add(new GuildMember(guild, guildNewcomer, GuildMemberType.Member));

            var newGithubUser = new GithubUser
            {
                IwentysUserId = guildNewcomer.Id,
                Username = guildNewcomer.GithubUsername
            };

            GithubUser githubUser = newGithubUser;
            GithubProject githubProject = context.GithubTestCaseContext.WithStudentProject(guildNewcomer, githubUser);
            var testTaskSolution = GuildTestTaskSolution.Create(guild, guildNewcomer);

            ProjectReviewRequest reviewRequest = ProjectReviewRequest.CreateGuildReviewRequest(guildNewcomer, githubProject, testTaskSolution, guild);
            testTaskSolution.SetCompleted(user);
            GuildTestTaskSolution userResponse = guild.TestTasks.First(t => t.AuthorId == guildNewcomer.Id);

            Assert.AreEqual(GuildTestTaskState.Completed, userResponse.GetState());
        }
    }
}
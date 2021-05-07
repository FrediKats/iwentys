using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Enums;
using Iwentys.Domain.Guilds.Models;
using Iwentys.Infrastructure.Application.Controllers.GuildTestTasks;
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
            //TODO: do not call directly
            guild.Members.Add(new GuildMember(guild, newMember, GuildMemberType.Member));

            var guildTestTaskSolution = GuildTestTaskSolution.Create(guild, newMember);

            Assert.IsTrue(guildTestTaskSolution.AuthorId == newMember.Id);
        }

        [Test]
        public async Task SubmitTestTask_StateShouldBeSubmitted()
        {
            TestCaseContext context = TestCaseContext.Case();
            AuthorizedUser user = context.AccountManagementTestCaseContext.WithUser();
            GuildProfileDto guild = context.GuildTestCaseContext.WithGuild(user);
            AuthorizedUser guildNewcomer = context.GuildTestCaseContext.WithGuildMember(guild, user);

            context.GithubTestCaseContext.WithGithubAccount(guildNewcomer);
            GithubProject githubProject = context.GithubTestCaseContext.WithStudentProject(guildNewcomer);

            await context.GuildTestTaskService.Accept(guildNewcomer, guild.Id);

            await new SubmitGuildTestTask.Handler(context.UnitOfWork, context.GithubIntegrationService).Handle(new SubmitGuildTestTask.Query(guildNewcomer, guild.Id, githubProject.Owner, githubProject.Name), CancellationToken.None);

            List<GuildTestTaskInfoResponse> taskInfoResponses = await context.GuildTestTaskService.GetResponses(guild.Id);
            GuildTestTaskInfoResponse userResponse = taskInfoResponses.First(t => t.StudentId == guildNewcomer.Id);

            Assert.AreEqual(GuildTestTaskState.Submitted, userResponse.TestTaskState);
        }

        [Test]
        public async Task CompleteTestTask_StateShouldBeCompleted()
        {
            TestCaseContext context = TestCaseContext.Case();
            AuthorizedUser user = context.AccountManagementTestCaseContext.WithUser();
            GuildProfileDto guild = context.GuildTestCaseContext.WithGuild(user);
            AuthorizedUser guildNewcomer = context.GuildTestCaseContext.WithGuildMember(guild, user);

            context.GithubTestCaseContext.WithGithubAccount(guildNewcomer);
            GithubProject githubProject = context.GithubTestCaseContext.WithStudentProject(guildNewcomer);

            await context.GuildTestTaskService.Accept(guildNewcomer, guild.Id);

            await new SubmitGuildTestTask.Handler(context.UnitOfWork, context.GithubIntegrationService).Handle(new SubmitGuildTestTask.Query(guildNewcomer, guild.Id, githubProject.Owner, githubProject.Name), CancellationToken.None);
            await context.GuildTestTaskService.Complete(user, guild.Id, guildNewcomer.Id);

            List<GuildTestTaskInfoResponse> taskInfoResponses = await context.GuildTestTaskService.GetResponses(guild.Id);
            GuildTestTaskInfoResponse userResponse = taskInfoResponses.First(t => t.StudentId == guildNewcomer.Id);

            Assert.AreEqual(GuildTestTaskState.Completed, userResponse.TestTaskState);
        }
    }
}
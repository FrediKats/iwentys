using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Guilds.Models;
using Iwentys.Tests.TestCaseContexts;
using NUnit.Framework;

namespace Iwentys.Tests.Features.Guilds
{
    [TestFixture]
    public class GuildTestTaskServiceTest
    {
        [Test]
        public async Task AcceptTestTask_ShouldBeInList()
        {
            TestCaseContext context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser guildCreator)
                .WithGuild(guildCreator, out ExtendedGuildProfileWithMemberDataDto guildProfile)
                .WithGuildMember(guildProfile, guildCreator, out AuthorizedUser guildNewcomer);

            await context.GuildTestTaskService.Accept(guildNewcomer, guildProfile.Id);
            List<GuildTestTaskInfoResponse> taskInfoResponses = await context.GuildTestTaskService.GetResponses(guildProfile.Id);

            Assert.IsTrue(taskInfoResponses.Any(t => t.StudentId == guildNewcomer.Id));
        }

        [Test]
        public async Task SubmitTestTask_StateShouldBeSubmitted()
        {
            TestCaseContext context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser guildCreator)
                .WithGuild(guildCreator, out ExtendedGuildProfileWithMemberDataDto guildProfile)
                .WithGuildMember(guildProfile, guildCreator, out AuthorizedUser guildNewcomer);
            context.GithubTestCaseContext.WithGithubAccount(guildNewcomer);
            GithubProject githubProject = context.GithubTestCaseContext.WithStudentProject(guildNewcomer);

            await context.GuildTestTaskService.Accept(guildNewcomer, guildProfile.Id);
            await context.GuildTestTaskService.Submit(guildNewcomer, guildProfile.Id, githubProject.Owner, githubProject.Name);

            List<GuildTestTaskInfoResponse> taskInfoResponses = await context.GuildTestTaskService.GetResponses(guildProfile.Id);
            GuildTestTaskInfoResponse userResponse = taskInfoResponses.First(t => t.StudentId == guildNewcomer.Id);

            Assert.AreEqual(GuildTestTaskState.Submitted, userResponse.TestTaskState);
        }

        [Test]
        public async Task CompleteTestTask_StateShouldBeCompleted()
        {
            TestCaseContext context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser guildCreator)
                .WithGuild(guildCreator, out ExtendedGuildProfileWithMemberDataDto guildProfile)
                .WithGuildMember(guildProfile, guildCreator, out AuthorizedUser guildNewcomer);
            context.GithubTestCaseContext.WithGithubAccount(guildNewcomer);
            GithubProject githubProject = context.GithubTestCaseContext.WithStudentProject(guildNewcomer);

            await context.GuildTestTaskService.Accept(guildNewcomer, guildProfile.Id);
            await context.GuildTestTaskService.Submit(guildNewcomer, guildProfile.Id, githubProject.Owner, githubProject.Name);
            await context.GuildTestTaskService.Complete(guildCreator, guildProfile.Id, guildNewcomer.Id);

            List<GuildTestTaskInfoResponse> taskInfoResponses = await context.GuildTestTaskService.GetResponses(guildProfile.Id);
            GuildTestTaskInfoResponse userResponse = taskInfoResponses.First(t => t.StudentId == guildNewcomer.Id);

            Assert.AreEqual(GuildTestTaskState.Completed, userResponse.TestTaskState);
        }
    }
}
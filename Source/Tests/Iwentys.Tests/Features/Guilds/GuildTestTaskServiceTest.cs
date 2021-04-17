﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Domain;
using Iwentys.Domain.Guilds.Enums;
using Iwentys.Domain.Models;
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
            TestCaseContext context = TestCaseContext.Case();
            AuthorizedUser user = context.AccountManagementTestCaseContext.WithUser();
            GuildProfileDto guild = context.GuildTestCaseContext.WithGuild(user);
            AuthorizedUser guildNewcomer = context.GuildTestCaseContext.WithGuildMember(guild, user);

            await context.GuildTestTaskService.Accept(guildNewcomer, guild.Id);
            List<GuildTestTaskInfoResponse> taskInfoResponses = await context.GuildTestTaskService.GetResponses(guild.Id);

            Assert.IsTrue(taskInfoResponses.Any(t => t.StudentId == guildNewcomer.Id));
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
            await context.GuildTestTaskService.Submit(guildNewcomer, guild.Id, githubProject.Owner, githubProject.Name);

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
            await context.GuildTestTaskService.Submit(guildNewcomer, guild.Id, githubProject.Owner, githubProject.Name);
            await context.GuildTestTaskService.Complete(user, guild.Id, guildNewcomer.Id);

            List<GuildTestTaskInfoResponse> taskInfoResponses = await context.GuildTestTaskService.GetResponses(guild.Id);
            GuildTestTaskInfoResponse userResponse = taskInfoResponses.First(t => t.StudentId == guildNewcomer.Id);

            Assert.AreEqual(GuildTestTaskState.Completed, userResponse.TestTaskState);
        }
    }
}
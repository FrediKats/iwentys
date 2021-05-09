using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Models;
using Iwentys.Infrastructure.DataAccess.Seeding.FakerEntities.Guilds;
using Iwentys.Tests.TestCaseContexts;
using NUnit.Framework;

namespace Iwentys.Tests.Features.Guilds
{
    [TestFixture]
    public class GuildTributeServiceTest
    {
        [Test]
        public void CreateTribute_TributeExists()
        {
            TestCaseContext context = TestCaseContext.Case();
            var student = context.AccountManagementTestCaseContext.WithIwentysUser();
            AuthorizedUser admin = context.AccountManagementTestCaseContext.WithUser(true);
            var guild = Guild.Create(student, null, GuildFaker.Instance.GetGuildCreateArguments());

            AuthorizedUser mentor = context.GuildTestCaseContext.WithGuildMentor(guild, student);

            GithubUser githubUser = context.GithubTestCaseContext.WithGithubAccount(student);
            GithubProject project = context.GithubTestCaseContext.WithStudentProject(student, githubUser);

            context.TributeTestCaseContext.WithTribute(student, project);
            List<TributeInfoResponse> tributes = context.GuildTributeServiceService.GetPendingTributes(mentor);

            Assert.IsNotEmpty(tributes);
            Assert.True(tributes.Any(t => t.Project.Id == project.Id));
        }

        [Test]
        public async Task CancelTribute_DoNotReturnForMentorAndReturnForStudent()
        {
            TestCaseContext context = TestCaseContext.Case();
            var student = context.AccountManagementTestCaseContext.WithIwentysUser();
            AuthorizedUser admin = context.AccountManagementTestCaseContext.WithUser(true);
            var guild = Guild.Create(student, null, GuildFaker.Instance.GetGuildCreateArguments());
            AuthorizedUser mentor = context.GuildTestCaseContext.WithGuildMentor(guild, student);

            GithubUser githubUser = context.GithubTestCaseContext.WithGithubAccount(student);
            GithubProject project = context.GithubTestCaseContext.WithStudentProject(student, githubUser);

            var tributeInfo = context.TributeTestCaseContext.WithTribute(student, project);
            await context.GuildTributeServiceService.CancelTribute(student, tributeInfo.Project.Id);
            List<TributeInfoResponse> pendingTributes = context.GuildTributeServiceService.GetPendingTributes(mentor);
            List<TributeInfoResponse> studentTributes = context.GuildTributeServiceService.GetStudentTributeResult(student);

            Assert.IsEmpty(pendingTributes);
            Assert.IsNotEmpty(studentTributes);
            Assert.True(studentTributes.Any(t => t.Project.Id == project.Id));
        }

        [Test]
        public void CompleteTribute_DoNotReturnForMentorAndChangeState()
        {
            TestCaseContext context = TestCaseContext.Case();
            var student = context.AccountManagementTestCaseContext.WithIwentysUser();
            AuthorizedUser admin = context.AccountManagementTestCaseContext.WithUser(true);
            var guild = Guild.Create(student, null, GuildFaker.Instance.GetGuildCreateArguments());
            AuthorizedUser mentor = context.GuildTestCaseContext.WithGuildMentor(guild, student);

            GithubUser githubUser = context.GithubTestCaseContext.WithGithubAccount(student);
            GithubProject project = context.GithubTestCaseContext.WithStudentProject(student, githubUser);

            var tributeInfo = context.TributeTestCaseContext.WithTribute(student, project);
            tributeInfo = context.TributeTestCaseContext.CompleteTribute(mentor, tributeInfo);
            List<TributeInfoResponse> pendingTributes = context.GuildTributeServiceService.GetPendingTributes(mentor);
            TributeInfoResponse studentTribute = context.GuildTributeServiceService.GetStudentTributeResult(student).FirstOrDefault(t => t.Project.Id == project.Id);

            Assert.IsEmpty(pendingTributes);
            Assert.NotNull(studentTribute);
            Assert.IsTrue(studentTribute.Mark == tributeInfo.Mark);
            Assert.IsTrue(studentTribute.DifficultLevel == tributeInfo.DifficultLevel);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.Guilds.Models;
using Iwentys.Features.Guilds.Tributes.Models;
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
            AuthorizedUser student = context.AccountManagementTestCaseContext.WithUser();
            AuthorizedUser admin = context.AccountManagementTestCaseContext.WithUser(true);
            ExtendedGuildProfileWithMemberDataDto guild = context.GuildTestCaseContext.WithGuild(student);
            AuthorizedUser mentor = context.GuildTestCaseContext.WithGuildMentor(guild);

            context.GithubTestCaseContext.WithGithubAccount(student);
            GithubProject project = context.GithubTestCaseContext.WithStudentProject(student);

            context.TributeTestCaseContext.WithTribute(student, project);
            List<TributeInfoResponse> tributes = context.GuildTributeServiceService.GetPendingTributes(mentor);

            Assert.IsNotEmpty(tributes);
            Assert.True(tributes.Any(t => t.Project.Id == project.Id));
        }

        [Test]
        public async Task CancelTribute_DoNotReturnForMentorAndReturnForStudent()
        {
            TestCaseContext context = TestCaseContext.Case();
            AuthorizedUser student = context.AccountManagementTestCaseContext.WithUser();
            AuthorizedUser admin = context.AccountManagementTestCaseContext.WithUser(true);
            ExtendedGuildProfileWithMemberDataDto guild = context.GuildTestCaseContext.WithGuild(student);
            AuthorizedUser mentor = context.GuildTestCaseContext.WithGuildMentor(guild);

            context.GithubTestCaseContext.WithGithubAccount(student);
            GithubProject project = context.GithubTestCaseContext.WithStudentProject(student);

            TributeInfoResponse tributeInfo = context.TributeTestCaseContext.WithTribute(student, project);
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
            AuthorizedUser student = context.AccountManagementTestCaseContext.WithUser();
            AuthorizedUser admin = context.AccountManagementTestCaseContext.WithUser(true);
            ExtendedGuildProfileWithMemberDataDto guild = context.GuildTestCaseContext.WithGuild(student);
            AuthorizedUser mentor = context.GuildTestCaseContext.WithGuildMentor(guild);

            context.GithubTestCaseContext.WithGithubAccount(student);
            GithubProject project = context.GithubTestCaseContext.WithStudentProject(student);

            TributeInfoResponse tributeInfo = context.TributeTestCaseContext.WithTribute(student, project);
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
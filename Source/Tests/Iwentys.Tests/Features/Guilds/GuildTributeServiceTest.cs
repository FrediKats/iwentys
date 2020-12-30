using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.Guilds.Models;
using Iwentys.Features.Guilds.Tributes.Models;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Enums;
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
            TestCaseContext context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser student)
                .WithGuild(student, out ExtendedGuildProfileWithMemberDataDto guild)
                .WithNewStudent(out AuthorizedUser admin, StudentRole.Admin)
                .WithMentor(guild, admin, out AuthorizedUser mentor)
                .WithStudentProject(student, out GithubProject project)
                .WithTribute(student, project, out TributeInfoResponse _);

            List<TributeInfoResponse> tributes = context.GuildTributeServiceService.GetPendingTributes(mentor);

            Assert.IsNotEmpty(tributes);
            Assert.True(tributes.Any(t => t.Project.Id == project.Id));
        }

        [Test]
        public async Task CancelTribute_DoNotReturnForMentorAndReturnForStudent()
        {
            TestCaseContext context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser student)
                .WithGuild(student, out ExtendedGuildProfileWithMemberDataDto guild)
                .WithNewStudent(out AuthorizedUser admin, StudentRole.Admin)
                .WithMentor(guild, admin, out AuthorizedUser mentor)
                .WithStudentProject(student, out GithubProject project)
                .WithTribute(student, project, out TributeInfoResponse tributeInfo);

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
            TestCaseContext context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser student)
                .WithGuild(student, out ExtendedGuildProfileWithMemberDataDto guild)
                .WithNewStudent(out AuthorizedUser admin, StudentRole.Admin)
                .WithMentor(guild, admin, out AuthorizedUser mentor)
                .WithStudentProject(student, out GithubProject project)
                .WithTribute(student, project, out TributeInfoResponse tributeInfo)
                .WithCompletedTribute(mentor, tributeInfo, out TributeInfoResponse completedTribute);

            List<TributeInfoResponse> pendingTributes = context.GuildTributeServiceService.GetPendingTributes(mentor);
            List<TributeInfoResponse> studentTributes = context.GuildTributeServiceService.GetStudentTributeResult(student);

            Assert.IsEmpty(pendingTributes);
            Assert.IsNotEmpty(studentTributes);

            TributeInfoResponse studentTribute = studentTributes.FirstOrDefault(t => t.Project.Id == project.Id);
            Assert.NotNull(studentTribute);
            Assert.IsTrue(studentTribute.Mark == completedTribute.Mark);
            Assert.IsTrue(studentTribute.DifficultLevel == completedTribute.DifficultLevel);
        }
    }
}
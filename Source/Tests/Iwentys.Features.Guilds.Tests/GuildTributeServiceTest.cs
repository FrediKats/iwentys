using System.Linq;
using System.Threading.Tasks;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.Guilds.ViewModels.Guilds;
using Iwentys.Features.Guilds.ViewModels.GuildTribute;
using Iwentys.Features.StudentFeature;
using Iwentys.Features.StudentFeature.Enums;
using Iwentys.Tests.Tools;
using NUnit.Framework;

namespace Iwentys.Features.Guilds.Tests
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
                .WithGuild(student, out GuildProfileDto guild)
                .WithNewStudent(out AuthorizedUser admin, UserType.Admin)
                .WithMentor(guild, admin, out AuthorizedUser mentor)
                .WithStudentProject(student, out GithubProjectEntity project)
                .WithTribute(student, project, out TributeInfoResponse _);

            TributeInfoResponse[] tributes = context.GuildTributeServiceService.GetPendingTributes(mentor);

            Assert.IsNotEmpty(tributes);
            Assert.True(tributes.Any(t => t.Project.Id == project.Id));
        }

        [Test]
        public async Task CancelTribute_DoNotReturnForMentorAndReturnForStudent()
        {
            TestCaseContext context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser student)
                .WithGuild(student, out GuildProfileDto guild)
                .WithNewStudent(out AuthorizedUser admin, UserType.Admin)
                .WithMentor(guild, admin, out AuthorizedUser mentor)
                .WithStudentProject(student, out GithubProjectEntity project)
                .WithTribute(student, project, out TributeInfoResponse tributeInfo);

            await context.GuildTributeServiceService.CancelTribute(student, tributeInfo.Project.Id);
            TributeInfoResponse[] pendingTributes = context.GuildTributeServiceService.GetPendingTributes(mentor);
            TributeInfoResponse[] studentTributes = context.GuildTributeServiceService.GetStudentTributeResult(student);

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
                .WithGuild(student, out GuildProfileDto guild)
                .WithNewStudent(out AuthorizedUser admin, UserType.Admin)
                .WithMentor(guild, admin, out AuthorizedUser mentor)
                .WithStudentProject(student, out GithubProjectEntity project)
                .WithTribute(student, project, out TributeInfoResponse tributeInfo)
                .WithCompletedTribute(mentor, tributeInfo, out TributeInfoResponse completedTribute);

            TributeInfoResponse[] pendingTributes = context.GuildTributeServiceService.GetPendingTributes(mentor);
            TributeInfoResponse[] studentTributes = context.GuildTributeServiceService.GetStudentTributeResult(student);

            Assert.IsEmpty(pendingTributes);
            Assert.IsNotEmpty(studentTributes);

            TributeInfoResponse studentTribute = studentTributes.FirstOrDefault(t => t.Project.Id == project.Id);
            Assert.NotNull(studentTribute);
            Assert.IsTrue(studentTribute.Mark == completedTribute.Mark);
            Assert.IsTrue(studentTribute.DifficultLevel == completedTribute.DifficultLevel);
        }
    }
}
using System.Linq;
using Iwentys.Core.DomainModel;
using Iwentys.Models.Entities;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Transferable.GuildTribute;
using Iwentys.Models.Types;
using Iwentys.Tests.Tools;
using NUnit.Framework;

namespace Iwentys.Tests.Core.Services
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
                .WithStudentProject(student, out StudentProject project)
                .WithTribute(student, project, out TributeInfoDto _);

            TributeInfoDto[] tributes = context.GuildTributeServiceService.GetPendingTributes(mentor);

            Assert.IsNotEmpty(tributes);
            Assert.True(tributes.Any(t => t.Project.Id == project.Id));
        }

        [Test]
        public void CancelTribute_DoNotReturnForMentorAndReturnForStudent()
        {
            TestCaseContext context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser student)
                .WithGuild(student, out GuildProfileDto guild)
                .WithNewStudent(out AuthorizedUser admin, UserType.Admin)
                .WithMentor(guild, admin, out AuthorizedUser mentor)
                .WithStudentProject(student, out StudentProject project)
                .WithTribute(student, project, out TributeInfoDto tributeInfo);

            context.GuildTributeServiceService.CancelTribute(student, tributeInfo.Project.Id);
            TributeInfoDto[] pendingTributes = context.GuildTributeServiceService.GetPendingTributes(mentor);
            TributeInfoDto[] studentTributes = context.GuildTributeServiceService.GetStudentTributeResult(student);

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
                .WithStudentProject(student, out StudentProject project)
                .WithTribute(student, project, out TributeInfoDto tributeInfo)
                .WithCompletedTribute(mentor, tributeInfo, out TributeInfoDto completedTribute);

            TributeInfoDto[] pendingTributes = context.GuildTributeServiceService.GetPendingTributes(mentor);
            TributeInfoDto[] studentTributes = context.GuildTributeServiceService.GetStudentTributeResult(student);

            Assert.IsEmpty(pendingTributes);
            Assert.IsNotEmpty(studentTributes);

            TributeInfoDto studentTribute = studentTributes.FirstOrDefault(t => t.Project.Id == project.Id);
            Assert.NotNull(studentTribute);
            Assert.IsTrue(studentTribute.Mark == completedTribute.Mark);
            Assert.IsTrue(studentTribute.DifficultLevel == completedTribute.DifficultLevel);
        }
    }
}
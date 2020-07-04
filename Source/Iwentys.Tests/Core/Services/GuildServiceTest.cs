using System.Linq;
using Iwentys.Core.DomainModel;
using Iwentys.Database.Repositories;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Types;
using Iwentys.Models.Types.Guilds;
using Iwentys.Tests.Tools;
using NUnit.Framework;

namespace Iwentys.Tests.Core.Services
{
    [TestFixture]
    public class GuildServiceTest
    {
        [Test]
        public void CreateGuild_ShouldReturnCreatorAsMember()
        {
            TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild);

            Assert.IsTrue(guild.Members.Any(m => m.Id == user.Id));
        }

        [Test]
        public void CreateGuild_GuildStateIsPending()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild);

            var createdGuild = context.GuildRepository.Get(guild.Id);

            Assert.AreEqual(GuildType.Pending, createdGuild.GuildType);
        }

        [Test]
        public void ApproveCreatedRepo_GuildStateIsCreated()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithNewStudent(out AuthorizedUser admin, UserType.Admin)
                .WithGuild(user, out GuildProfileDto guild);

            context.GuildService.ApproveGuildCreating(admin, guild.Id);
            var createdGuild = context.GuildRepository.Get(guild.Id);

            Assert.AreEqual(GuildType.Created, createdGuild.GuildType);
        }

        [Test]
        public void CreateTwoGuildForUser_ErrorUserAlreadyInGuild()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithNewStudent(out AuthorizedUser _, UserType.Admin)
                .WithGuild(user, out GuildProfileDto _);

            Assert.Catch<InnerLogicException>(() => context.WithGuild(user, out GuildProfileDto _));
        }

        [Test]
        public void CreateTribute_TributeExists()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild)
                .WithNewStudent(out AuthorizedUser admin, UserType.Admin)
                .WithTotem(guild, admin, out AuthorizedUser totem)
                .WithStudentProject(user, out StudentProject project)
                .WithTribute(user, project, out Tribute _);

            Tribute[] tributes = context.GuildService.GetTributes(totem);
            
            Assert.IsNotEmpty(tributes);
            Assert.True(tributes.Any(t => t.ProjectId == project.Id));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Iwentys.Common.Tools;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Guilds.Models.Guilds;
using Iwentys.Features.StudentFeature.Domain;
using Iwentys.Features.StudentFeature.Enums;
using Iwentys.Tests.Tools;
using NUnit.Framework;

namespace Iwentys.Features.Guilds.Tests
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

            bool isExist = guild.MemberLeaderBoard.Members.Any(_ => _.Id == user.Id);
            Assert.IsTrue(isExist);
        }

        [Test]
        public void CreateGuild_GuildStateIsPending()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild);

            var createdGuild = context.GuildRepository.GetAsync(guild.Id);

            Assert.AreEqual(GuildType.Pending, createdGuild.Result.GuildType);
        }

        [Test]
        public async Task ApproveCreatedRepo_GuildStateIsCreated()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithNewStudent(out AuthorizedUser admin, UserType.Admin)
                .WithGuild(user, out GuildProfileDto guild);

            await context.GuildService.ApproveGuildCreating(admin, guild.Id);
            GuildEntity createdGuild = await context.GuildRepository.GetAsync(guild.Id);

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

            //TODO: rework to correct exception
            Assert.Catch<AggregateException>(() => context.WithGuild(user, out GuildProfileDto _));
        }

        [Test]
        public void GetGuildRequests_ForGuildEditorAndNoRequestsToGuild_EmptyGildMembersArray()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild);

            List<GuildMemberEntity> requests = context.GuildMemberService.GetGuildRequests(user, guild.Id).Result.ToList();

            Assert.That(requests, Is.Not.Null);
            Assert.That(requests.Any(), Is.False);
        }

        [Test]
        public void GetGuildRequests_ForNotGuildMember_ThrowInnerLogicException()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild)
                .WithNewStudent(out AuthorizedUser student);

            Assert.ThrowsAsync<InnerLogicException>(() => context.GuildMemberService.GetGuildRequests(student, guild.Id));
        }

        [Test]
        public void GetGuildRequests_ForGuildMemberWithoutEditorPermissions_ThrowInnerLogicException()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild)
                .WithGuildMember(guild, out AuthorizedUser member);

            Assert.ThrowsAsync<InnerLogicException>(() => context.GuildMemberService.GetGuildRequests(member, guild.Id));
        }

        [Test]
        public void GetGuildRequests_ForGuildEditor_GildMembersArrayWithRequestStatus()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild)
                .WithGuildRequest(guild, out AuthorizedUser student);

            List<GuildMemberEntity> requests = context.GuildMemberService.GetGuildRequests(user, guild.Id).Result.ToList();

            Assert.That(requests, Is.Not.Null);
            Assert.That(requests.Length, Is.EqualTo(1));
            Assert.That(requests[0].MemberId, Is.EqualTo(student.Id));
            Assert.That(requests[0].MemberType, Is.EqualTo(GuildMemberType.Requested));
        }

        [Test]
        public void GetGuildBlocked_ForGuildEditor_GildMembersArrayWithBlockedStatus()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild)
                .WithGuildBlocked(guild, out AuthorizedUser student);

            List<GuildMemberEntity> blocked = context.GuildMemberService.GetGuildBlocked(user, guild.Id).Result.ToList();

            Assert.That(blocked, Is.Not.Null);
            Assert.That(blocked.Length, Is.EqualTo(1));
            Assert.That(blocked[0].MemberId, Is.EqualTo(student.Id));
            Assert.That(blocked[0].MemberType, Is.EqualTo(GuildMemberType.Blocked));
        }

        [Test]
        public async Task BlockGuildMember_AddUserToBlockedListAndKickFromGuild()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild)
                .WithGuildMember(guild, out AuthorizedUser member);

            await context.GuildMemberService.BlockGuildMember(user, guild.Id, member.Id);
            GuildMemberEntity[] blocked = await context.GuildMemberService.GetGuildBlocked(user, guild.Id);
            GuildEntity memberGuild = context.GuildRepository.ReadForStudent(member.Id);

            Assert.That(blocked.Find(m => m.MemberId == member.Id), Is.Not.Null);
            Assert.That(memberGuild, Is.Null);
        }

        [Test]
        public void BlockGuildMember_BlockCreator_ThrowsInnerLogicException()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild);

            Assert.ThrowsAsync<InnerLogicException>(() => context.GuildMemberService.BlockGuildMember(user, guild.Id, user.Id));
        }

        [Test]
        public void BlockGuildMember_BlockNotGuildMember_ThrowsInnerLogicException()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild)
                .WithNewStudent(out AuthorizedUser student);

            Assert.ThrowsAsync<InnerLogicException>(() => context.GuildMemberService.BlockGuildMember(user, guild.Id, student.Id));
        }

        [Test]
        public void BlockGuildMember_BlockMentorByMentor_ThrowsInnerLogicException()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild)
                .WithGuildMentor(guild, out AuthorizedUser mentor)
                .WithGuildMentor(guild, out AuthorizedUser anotherMentor);

            Assert.ThrowsAsync<InnerLogicException>(() => context.GuildMemberService.BlockGuildMember(mentor, guild.Id, anotherMentor.Id));
        }

        [Test]
        public async Task KickGuildMember_KickMemberFromGuild()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild)
                .WithGuildMember(guild, out AuthorizedUser member);

            await context.GuildMemberService.KickGuildMemberAsync(user, guild.Id, member.Id);
            GuildEntity memberGuild = context.GuildRepository.ReadForStudent(member.Id);

            Assert.That(memberGuild, Is.Null);
        }

        [Test]
        public void KickGuildMember_KickMentorByMentor_ThrowsInnerLogicException()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild)
                .WithGuildMentor(guild, out AuthorizedUser mentor)
                .WithGuildMentor(guild, out AuthorizedUser anotherMentor);

            Assert.ThrowsAsync<InnerLogicException>(() =>  context.GuildMemberService.KickGuildMemberAsync(mentor, guild.Id, anotherMentor.Id));
        }

        [Test]
        public async Task UnblockStudent_RemoveStudentFromListOfBlocked()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild)
                .WithGuildBlocked(guild, out AuthorizedUser student);

            await context.GuildMemberService.UnblockStudent(user, guild.Id, student.Id);
            GuildMemberEntity[] blocked = await context.GuildMemberService.GetGuildBlocked(user, guild.Id);

            Assert.That(blocked.FirstOrDefault(m => m.MemberId == student.Id), Is.Null);
        }

        [Test]
        public void UnblockStudent_NotBlockedInGuild_ThrowsInnerLogicException()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild)
                .WithNewStudent(out AuthorizedUser student);

            Assert.ThrowsAsync<InnerLogicException>(() => context.GuildMemberService.UnblockStudent(user, guild.Id, student.Id));
        }

        [Test]
        public async Task AcceptRequest_RemoveFromRequestListAndAddToMembers()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild)
                .WithGuildRequest(guild, out AuthorizedUser student);

            await context.GuildMemberService.AcceptRequest(user, guild.Id, student.Id);
            GuildEntity guildEntity = await context.GuildRepository.GetAsync(guild.Id);
            GuildMemberEntity member = guildEntity.Members.Find(m => m.MemberId == student.Id);
            GuildMemberEntity[] requests = await context.GuildMemberService.GetGuildRequests(user, guild.Id);

            Assert.IsNotNull(member);
            Assert.That(member.MemberType, Is.EqualTo(GuildMemberType.Member));
            Assert.That(requests.FirstOrDefault(m => m.MemberId == student.Id), Is.Null);
        }

        [Test]
        public void AcceptRequest_ForStudentWithoutRequest_ThrowsInnerLogicException()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild)
                .WithNewStudent(out AuthorizedUser student);

            Assert.ThrowsAsync<InnerLogicException>(() => context.GuildMemberService.AcceptRequest(user, guild.Id, student.Id));
        }

        [Test]
        public async Task RejectRequest_RemoveFromRequestList()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild)
                .WithGuildRequest(guild, out AuthorizedUser student);

            await context.GuildMemberService.RejectRequest(user, guild.Id, student.Id);
            GuildEntity guildEntity = await context.GuildRepository.GetAsync(guild.Id);
            GuildMemberEntity member = guildEntity.Members.Find(m => m.MemberId == student.Id);
            GuildMemberEntity[] requests = await context.GuildMemberService.GetGuildRequests(user, guild.Id);

            Assert.That(member, Is.Null);
            Assert.That(requests.FirstOrDefault(m => m.MemberId == student.Id), Is.Null);
        }

        [Test]
        public void RejectRequest_ForStudentWithoutRequest_ThrowsInnerLogicException()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild)
                .WithNewStudent(out AuthorizedUser student);

            Assert.ThrowsAsync<InnerLogicException>(() => context.GuildMemberService.RejectRequest(user, guild.Id, student.Id));
        }

        [Test]
        public async Task UpdateGuild_UpdateHiringPolicyToClose_CloseGuild()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild)
                .WithGuildRequest(guild, out AuthorizedUser _);
            await context.GuildService.UpdateAsync(user, new GuildUpdateRequest { Id = guild.Id, HiringPolicy = GuildHiringPolicy.Close });

            GuildEntity guildEntity = await context.GuildRepository.GetAsync(guild.Id);
            Assert.That(guildEntity.HiringPolicy, Is.EqualTo(GuildHiringPolicy.Close));
        }

        [Test]
        public async Task UpdateGuild_UpdateHiringPolicyToOpen_SwitchRequestsToMembers()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild)
                .WithGuildRequest(guild, out AuthorizedUser student);
            await context.GuildService.UpdateAsync(user, new GuildUpdateRequest() { Id = guild.Id, HiringPolicy = GuildHiringPolicy.Close });
            await context.GuildService.UpdateAsync(user, new GuildUpdateRequest() { Id = guild.Id, HiringPolicy = GuildHiringPolicy.Open });

            GuildEntity guildEntity = await context.GuildRepository.GetAsync(guild.Id);
            GuildMemberEntity newMember = guildEntity.Members.Find(m => m.MemberId == student.Id);
            Assert.IsNotNull(newMember);
            Assert.That(newMember.MemberType, Is.EqualTo(GuildMemberType.Member));
        }

        [Test]

        public void GetGuildMemberLeaderBoard()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGithubRepository(user, out GithubUserEntity userData)
                .WithGuild(user, out GuildProfileDto guild);

            Assert.That(context.GuildService.GetGuildMemberLeaderBoard(guild.Id).Result.MembersImpact.Single().TotalRate,
                Is.EqualTo(userData.ContributionFullInfo.Total));
        }
    }
}
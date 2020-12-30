using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Guilds.Models;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Enums;
using Iwentys.Tests.TestCaseContexts;
using NUnit.Framework;

namespace Iwentys.Tests.Features.Guilds
{
    [TestFixture]
    public class GuildServiceTest
    {
        [Test]
        public async Task CreateGuild_ShouldReturnCreatorAsMember()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out ExtendedGuildProfileWithMemberDataDto guild);

            var guildMemberLeaderBoardDto = await context.GuildService.GetGuildMemberLeaderBoard(guild.Id);
            bool isExist = guildMemberLeaderBoardDto.MembersImpact.Any(_ => _.StudentInfoDto.Id == user.Id);
            Assert.IsTrue(isExist);
        }

        [Test]
        [Ignore("Meh (")]
        public void CreateGuild_GuildStateIsPending()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out ExtendedGuildProfileWithMemberDataDto guild);

            //var createdGuild = context.GuildRepository.Get(guild.Id);

            //Assert.AreEqual(GuildType.Pending, createdGuild.Result.GuildType);
        }

        [Test]
        [Ignore("Meh (")]
        public async Task ApproveCreatedRepo_GuildStateIsCreated()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithNewStudent(out AuthorizedUser admin, StudentRole.Admin)
                .WithGuild(user, out ExtendedGuildProfileWithMemberDataDto guild);

            await context.GuildService.ApproveGuildCreating(admin, guild.Id);
            //Guild createdGuild = await context.GuildRepository.Get(guild.Id);

            //Assert.AreEqual(GuildType.Created, createdGuild.GuildType);
        }

        [Test]
        public void CreateTwoGuildForUser_ErrorUserAlreadyInGuild()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithNewStudent(out AuthorizedUser _, StudentRole.Admin)
                .WithGuild(user, out ExtendedGuildProfileWithMemberDataDto _);

            //TODO: rework to correct exception
            Assert.Catch<AggregateException>(() => context.WithGuild(user, out ExtendedGuildProfileWithMemberDataDto _));
        }

        [Test]
        public void GetGuildRequests_ForGuildEditorAndNoRequestsToGuild_EmptyGildMembersArray()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out ExtendedGuildProfileWithMemberDataDto guild);

            List<GuildMember> requests = context.GuildMemberService.GetGuildRequests(user, guild.Id).Result.ToList();

            Assert.That(requests, Is.Not.Null);
            Assert.That(requests.Any(), Is.False);
        }

        [Test]
        public void GetGuildRequests_ForNotGuildMember_ThrowInnerLogicException()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out ExtendedGuildProfileWithMemberDataDto guild)
                .WithNewStudent(out AuthorizedUser student);

            Assert.ThrowsAsync<InnerLogicException>(() => context.GuildMemberService.GetGuildRequests(student, guild.Id));
        }

        [Test]
        public void GetGuildRequests_ForGuildMemberWithoutEditorPermissions_ThrowInnerLogicException()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser guildCreator)
                .WithGuild(guildCreator, out ExtendedGuildProfileWithMemberDataDto guild)
                .WithGuildMember(guild, guildCreator, out AuthorizedUser member);

            Assert.ThrowsAsync<InnerLogicException>(() => context.GuildMemberService.GetGuildRequests(member, guild.Id));
        }

        [Test]
        public void GetGuildRequests_ForGuildEditor_GildMembersArrayWithRequestStatus()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out ExtendedGuildProfileWithMemberDataDto guild)
                .WithGuildRequest(guild, out AuthorizedUser student);

            List<GuildMember> requests = context.GuildMemberService.GetGuildRequests(user, guild.Id).Result.ToList();

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
                .WithNewStudent(out AuthorizedUser guildCreator)
                .WithGuild(guildCreator, out ExtendedGuildProfileWithMemberDataDto guild)
                .WithGuildBlocked(guild, guildCreator, out AuthorizedUser student);

            List<GuildMember> blocked = context.GuildMemberService.GetGuildBlocked(guildCreator, guild.Id).Result.ToList();

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
                .WithNewStudent(out AuthorizedUser guildCreator)
                .WithGuild(guildCreator, out ExtendedGuildProfileWithMemberDataDto guild)
                .WithGuildMember(guild, guildCreator, out AuthorizedUser member);

            await context.GuildMemberService.BlockGuildMember(guildCreator, guild.Id, member.Id);
            GuildMember[] blocked = await context.GuildMemberService.GetGuildBlocked(guildCreator, guild.Id);
            
            var memberGuild = context.GuildService.FindStudentGuild(member.Id);

            Assert.That(blocked.Find(m => m.MemberId == member.Id), Is.Not.Null);
            Assert.That(memberGuild, Is.Null);
        }

        [Test]
        public void BlockGuildMember_BlockCreator_ThrowsInnerLogicException()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out ExtendedGuildProfileWithMemberDataDto guild);

            Assert.ThrowsAsync<InnerLogicException>(() => context.GuildMemberService.BlockGuildMember(user, guild.Id, user.Id));
        }

        [Test]
        public void BlockGuildMember_BlockNotGuildMember_ThrowsInnerLogicException()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out ExtendedGuildProfileWithMemberDataDto guild)
                .WithNewStudent(out AuthorizedUser student);

            Assert.ThrowsAsync<InnerLogicException>(() => context.GuildMemberService.BlockGuildMember(user, guild.Id, student.Id));
        }

        [Test]
        public void BlockGuildMember_BlockMentorByMentor_ThrowsInnerLogicException()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out ExtendedGuildProfileWithMemberDataDto guild)
                .WithGuildMentor(guild, out AuthorizedUser mentor)
                .WithGuildMentor(guild, out AuthorizedUser anotherMentor);

            Assert.ThrowsAsync<InnerLogicException>(() => context.GuildMemberService.BlockGuildMember(mentor, guild.Id, anotherMentor.Id));
        }

        [Test]
        public async Task KickGuildMember_KickMemberFromGuild()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser guildCreator)
                .WithGuild(guildCreator, out ExtendedGuildProfileWithMemberDataDto guild)
                .WithGuildMember(guild, guildCreator, out AuthorizedUser member);

            await context.GuildMemberService.KickGuildMemberAsync(guildCreator, guild.Id, member.Id);
            GuildProfileDto memberGuild = context.GuildService.FindStudentGuild(member.Id);

            Assert.That(memberGuild, Is.Null);
        }

        [Test]
        public void KickGuildMember_KickMentorByMentor_ThrowsInnerLogicException()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out ExtendedGuildProfileWithMemberDataDto guild)
                .WithGuildMentor(guild, out AuthorizedUser mentor)
                .WithGuildMentor(guild, out AuthorizedUser anotherMentor);

            Assert.ThrowsAsync<InnerLogicException>(() =>  context.GuildMemberService.KickGuildMemberAsync(mentor, guild.Id, anotherMentor.Id));
        }

        [Test]
        public async Task UnblockStudent_RemoveStudentFromListOfBlocked()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser guildCreator)
                .WithGuild(guildCreator, out ExtendedGuildProfileWithMemberDataDto guild)
                .WithGuildBlocked(guild, guildCreator, out AuthorizedUser student);

            await context.GuildMemberService.UnblockStudent(guildCreator, guild.Id, student.Id);
            GuildMember[] blocked = await context.GuildMemberService.GetGuildBlocked(guildCreator, guild.Id);

            Assert.That(blocked.FirstOrDefault(m => m.MemberId == student.Id), Is.Null);
        }

        [Test]
        public void UnblockStudent_NotBlockedInGuild_ThrowsInnerLogicException()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out ExtendedGuildProfileWithMemberDataDto guild)
                .WithNewStudent(out AuthorizedUser student);

            Assert.ThrowsAsync<InnerLogicException>(() => context.GuildMemberService.UnblockStudent(user, guild.Id, student.Id));
        }

        [Test]
        public async Task AcceptRequest_RemoveFromRequestListAndAddToMembers()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out ExtendedGuildProfileWithMemberDataDto guildDto)
                .WithGuildRequest(guildDto, out AuthorizedUser student);

            await context.GuildMemberService.AcceptRequest(user, guildDto.Id, student.Id);
            var guildMemberLeaderBoardDto = await context.GuildService.GetGuildMemberLeaderBoard(guildDto.Id);
            var member = guildMemberLeaderBoardDto.MembersImpact.Find(m => m.StudentInfoDto.Id == student.Id);
            GuildMember[] requests = await context.GuildMemberService.GetGuildRequests(user, guildDto.Id);

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
                .WithGuild(user, out ExtendedGuildProfileWithMemberDataDto guild)
                .WithNewStudent(out AuthorizedUser student);

            Assert.ThrowsAsync<InnerLogicException>(() => context.GuildMemberService.AcceptRequest(user, guild.Id, student.Id));
        }

        [Test]
        public async Task RejectRequest_RemoveFromRequestList()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out ExtendedGuildProfileWithMemberDataDto guildDto)
                .WithGuildRequest(guildDto, out AuthorizedUser student);

            await context.GuildMemberService.RejectRequest(user, guildDto.Id, student.Id);
            var guildMemberLeaderBoardDto = await context.GuildService.GetGuildMemberLeaderBoard(guildDto.Id);
            var member = guildMemberLeaderBoardDto.MembersImpact.Find(m => m.StudentInfoDto.Id == student.Id);
            GuildMember[] requests = await context.GuildMemberService.GetGuildRequests(user, guildDto.Id);

            Assert.That(member, Is.Null);
            Assert.That(requests.FirstOrDefault(m => m.MemberId == student.Id), Is.Null);
        }

        [Test]
        public void RejectRequest_ForStudentWithoutRequest_ThrowsInnerLogicException()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out ExtendedGuildProfileWithMemberDataDto guild)
                .WithNewStudent(out AuthorizedUser student);

            Assert.ThrowsAsync<InnerLogicException>(() => context.GuildMemberService.RejectRequest(user, guild.Id, student.Id));
        }

        [Test]
        public async Task UpdateGuild_UpdateHiringPolicyToClose_CloseGuild()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out ExtendedGuildProfileWithMemberDataDto guild)
                .WithGuildRequest(guild, out AuthorizedUser _);
            await context.GuildService.UpdateAsync(user, GuildUpdateRequestDto.ForPolicyUpdate(guild.Id, GuildHiringPolicy.Close));

            var guildEntity = await context.GuildService.GetAsync(guild.Id, null);
            Assert.That(guildEntity.HiringPolicy, Is.EqualTo(GuildHiringPolicy.Close));
        }

        [Test]
        public async Task UpdateGuild_UpdateHiringPolicyToOpen_SwitchRequestsToMembers()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out ExtendedGuildProfileWithMemberDataDto guildDto)
                .WithGuildRequest(guildDto, out AuthorizedUser student);
            await context.GuildService.UpdateAsync(user, GuildUpdateRequestDto.ForPolicyUpdate(guildDto.Id, GuildHiringPolicy.Close));
            await context.GuildService.UpdateAsync(user, GuildUpdateRequestDto.ForPolicyUpdate(guildDto.Id, GuildHiringPolicy.Open));

            var guildMemberLeaderBoardDto = await context.GuildService.GetGuildMemberLeaderBoard(guildDto.Id);
            var newMember = guildMemberLeaderBoardDto.MembersImpact.Find(m => m.StudentInfoDto.Id == student.Id);
            Assert.IsNotNull(newMember);
            Assert.That(newMember.MemberType, Is.EqualTo(GuildMemberType.Member));
        }

        [Test]

        public void GetGuildMemberLeaderBoard()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGithubRepository(user, out GithubUser userData)
                .WithGuild(user, out ExtendedGuildProfileWithMemberDataDto guild);

            Assert.That(context.GuildService.GetGuildMemberLeaderBoard(guild.Id).Result.MembersImpact.Single().TotalRate,
                Is.EqualTo(userData.ContributionFullInfo.Total));
        }
    }
}
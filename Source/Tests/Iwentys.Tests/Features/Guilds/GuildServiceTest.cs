using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Iwentys.Domain;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Enums;
using Iwentys.Domain.Models;
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
            TestCaseContext context = TestCaseContext.Case();
            AuthorizedUser user = context.AccountManagementTestCaseContext.WithUser();
            GuildProfileDto guild = context.GuildTestCaseContext.WithGuild(user);

            GuildMemberLeaderBoardDto guildMemberLeaderBoardDto = await context.GuildService.GetGuildMemberLeaderBoard(guild.Id);
            var isExist = guildMemberLeaderBoardDto.MembersImpact.Any(_ => _.StudentInfoDto.Id == user.Id);
            Assert.IsTrue(isExist);
        }

        [Test]
        public async Task CreateGuild_GuildStateIsPending()
        {
            TestCaseContext context = TestCaseContext.Case();
            AuthorizedUser user = context.AccountManagementTestCaseContext.WithUser();
            GuildProfileDto guild = context.GuildTestCaseContext.WithGuild(user);

            GuildProfileDto createdGuild = await context.GuildService.Get(guild.Id);

            Assert.AreEqual(GuildType.Pending, createdGuild.GuildType);
        }

        [Test]
        public async Task ApproveCreatedRepo_GuildStateIsCreated()
        {
            TestCaseContext context = TestCaseContext.Case();
            AuthorizedUser user = context.AccountManagementTestCaseContext.WithUser();
            AuthorizedUser admin = context.AccountManagementTestCaseContext.WithUser(true);
            GuildProfileDto guild = context.GuildTestCaseContext.WithGuild(user);

            await context.GuildService.ApproveGuildCreating(admin, guild.Id);
            GuildProfileDto createdGuild = await context.GuildService.Get(guild.Id);

            Assert.AreEqual(GuildType.Created, createdGuild.GuildType);
        }

        [Test]
        public void CreateTwoGuildForUser_ErrorUserAlreadyInGuild()
        {
            TestCaseContext context = TestCaseContext.Case();
            AuthorizedUser user = context.AccountManagementTestCaseContext.WithUser();
            AuthorizedUser admin = context.AccountManagementTestCaseContext.WithUser(true);
            GuildProfileDto guild = context.GuildTestCaseContext.WithGuild(user);

            //TODO: rework to correct exception
            Assert.Catch<AggregateException>(() => context.GuildTestCaseContext.WithGuild(user));
        }

        [Test]
        public void GetGuildRequests_ForGuildEditorAndNoRequestsToGuild_EmptyGildMembersArray()
        {
            TestCaseContext context = TestCaseContext.Case();
            AuthorizedUser user = context.AccountManagementTestCaseContext.WithUser();
            AuthorizedUser admin = context.AccountManagementTestCaseContext.WithUser(true);
            GuildProfileDto guild = context.GuildTestCaseContext.WithGuild(user);

            List<GuildMember> requests = context.GuildMemberService.GetGuildRequests(user, guild.Id).Result.ToList();

            Assert.That(requests, Is.Not.Null);
            Assert.That(requests.Any(), Is.False);
        }

        [Test]
        public void GetGuildRequests_ForNotGuildMember_ThrowInnerLogicException()
        {
            TestCaseContext context = TestCaseContext.Case();
            AuthorizedUser user = context.AccountManagementTestCaseContext.WithUser();
            AuthorizedUser student = context.AccountManagementTestCaseContext.WithUser();
            GuildProfileDto guild = context.GuildTestCaseContext.WithGuild(user);

            Assert.ThrowsAsync<InnerLogicException>(() => context.GuildMemberService.GetGuildRequests(student, guild.Id));
        }

        [Test]
        public void GetGuildRequests_ForGuildMemberWithoutEditorPermissions_ThrowInnerLogicException()
        {
            TestCaseContext context = TestCaseContext.Case();
            AuthorizedUser user = context.AccountManagementTestCaseContext.WithUser();
            GuildProfileDto guild = context.GuildTestCaseContext.WithGuild(user);
            AuthorizedUser member = context.GuildTestCaseContext.WithGuildMember(guild, user);

            Assert.ThrowsAsync<InnerLogicException>(() => context.GuildMemberService.GetGuildRequests(member, guild.Id));
        }

        [Test]
        public void GetGuildRequests_ForGuildEditor_GildMembersArrayWithRequestStatus()
        {
            TestCaseContext context = TestCaseContext.Case();
            AuthorizedUser user = context.AccountManagementTestCaseContext.WithUser();
            GuildProfileDto guild = context.GuildTestCaseContext.WithGuild(user);
            AuthorizedUser member = context.GuildTestCaseContext.WithGuildRequest(guild);

            List<GuildMember> requests = context.GuildMemberService.GetGuildRequests(user, guild.Id).Result.ToList();

            Assert.That(requests, Is.Not.Null);
            Assert.That(requests.Length, Is.EqualTo(1));
            Assert.That(requests[0].MemberId, Is.EqualTo(member.Id));
            Assert.That(requests[0].MemberType, Is.EqualTo(GuildMemberType.Requested));
        }

        [Test]
        public void GetGuildBlocked_ForGuildEditor_GildMembersArrayWithBlockedStatus()
        {
            TestCaseContext context = TestCaseContext.Case();
            AuthorizedUser user = context.AccountManagementTestCaseContext.WithUser();
            GuildProfileDto guild = context.GuildTestCaseContext.WithGuild(user);
            AuthorizedUser member = context.GuildTestCaseContext.WithGuildBlocked(guild, user);

            List<GuildMember> blocked = context.GuildMemberService.GetGuildBlocked(user, guild.Id).Result.ToList();

            Assert.That(blocked, Is.Not.Null);
            Assert.That(blocked.Length, Is.EqualTo(1));
            Assert.That(blocked[0].MemberId, Is.EqualTo(member.Id));
            Assert.That(blocked[0].MemberType, Is.EqualTo(GuildMemberType.Blocked));
        }

        [Test]
        public async Task BlockGuildMember_AddUserToBlockedListAndKickFromGuild()
        {
            TestCaseContext context = TestCaseContext.Case();
            AuthorizedUser user = context.AccountManagementTestCaseContext.WithUser();
            GuildProfileDto guild = context.GuildTestCaseContext.WithGuild(user);
            AuthorizedUser member = context.GuildTestCaseContext.WithGuildMember(guild, user);

            await context.GuildMemberService.BlockGuildMember(user, guild.Id, member.Id);
            GuildMember[] blocked = await context.GuildMemberService.GetGuildBlocked(user, guild.Id);

            GuildProfileDto memberGuild = context.GuildService.FindStudentGuild(member.Id);

            Assert.That(blocked.Find(m => m.MemberId == member.Id), Is.Not.Null);
            Assert.That(memberGuild, Is.Null);
        }

        [Test]
        public void BlockGuildMember_BlockCreator_ThrowsInnerLogicException()
        {
            TestCaseContext context = TestCaseContext.Case();
            AuthorizedUser user = context.AccountManagementTestCaseContext.WithUser();
            GuildProfileDto guild = context.GuildTestCaseContext.WithGuild(user);

            Assert.ThrowsAsync<InnerLogicException>(() => context.GuildMemberService.BlockGuildMember(user, guild.Id, user.Id));
        }

        [Test]
        public void BlockGuildMember_BlockNotGuildMember_ThrowsInnerLogicException()
        {
            TestCaseContext context = TestCaseContext.Case();
            AuthorizedUser user = context.AccountManagementTestCaseContext.WithUser();
            GuildProfileDto guild = context.GuildTestCaseContext.WithGuild(user);
            AuthorizedUser student = context.AccountManagementTestCaseContext.WithUser();

            Assert.ThrowsAsync<InnerLogicException>(() => context.GuildMemberService.BlockGuildMember(user, guild.Id, student.Id));
        }

        [Test]
        public void BlockGuildMember_BlockMentorByMentor_ThrowsInnerLogicException()
        {
            TestCaseContext context = TestCaseContext.Case();
            AuthorizedUser user = context.AccountManagementTestCaseContext.WithUser();
            GuildProfileDto guild = context.GuildTestCaseContext.WithGuild(user);
            AuthorizedUser mentor = context.GuildTestCaseContext.WithGuildMentor(guild, user);
            AuthorizedUser anotherMentor = context.GuildTestCaseContext.WithGuildMentor(guild, user);

            Assert.ThrowsAsync<InnerLogicException>(() => context.GuildMemberService.BlockGuildMember(mentor, guild.Id, anotherMentor.Id));
        }

        [Test]
        public async Task KickGuildMember_KickMemberFromGuild()
        {
            TestCaseContext context = TestCaseContext.Case();
            AuthorizedUser user = context.AccountManagementTestCaseContext.WithUser();
            GuildProfileDto guild = context.GuildTestCaseContext.WithGuild(user);
            AuthorizedUser member = context.GuildTestCaseContext.WithGuildMember(guild, user);

            await context.GuildMemberService.KickGuildMember(user, guild.Id, member.Id);
            GuildProfileDto memberGuild = context.GuildService.FindStudentGuild(member.Id);

            Assert.That(memberGuild, Is.Null);
        }

        [Test]
        public void KickGuildMember_KickMentorByMentor_ThrowsInnerLogicException()
        {
            TestCaseContext context = TestCaseContext.Case();
            AuthorizedUser user = context.AccountManagementTestCaseContext.WithUser();
            GuildProfileDto guild = context.GuildTestCaseContext.WithGuild(user);
            AuthorizedUser mentor = context.GuildTestCaseContext.WithGuildMentor(guild, user);
            AuthorizedUser anotherMentor = context.GuildTestCaseContext.WithGuildMentor(guild, user);

            Assert.ThrowsAsync<InnerLogicException>(() => context.GuildMemberService.KickGuildMember(mentor, guild.Id, anotherMentor.Id));
        }

        [Test]
        public async Task UnblockStudent_RemoveStudentFromListOfBlocked()
        {
            TestCaseContext context = TestCaseContext.Case();
            AuthorizedUser user = context.AccountManagementTestCaseContext.WithUser();
            GuildProfileDto guild = context.GuildTestCaseContext.WithGuild(user);
            AuthorizedUser student = context.GuildTestCaseContext.WithGuildBlocked(guild, user);

            await context.GuildMemberService.UnblockStudent(user, guild.Id, student.Id);
            GuildMember[] blocked = await context.GuildMemberService.GetGuildBlocked(user, guild.Id);

            Assert.That(blocked.FirstOrDefault(m => m.MemberId == student.Id), Is.Null);
        }

        [Test]
        public void UnblockStudent_NotBlockedInGuild_ThrowsInnerLogicException()
        {
            TestCaseContext context = TestCaseContext.Case();
            AuthorizedUser user = context.AccountManagementTestCaseContext.WithUser();
            GuildProfileDto guild = context.GuildTestCaseContext.WithGuild(user);
            AuthorizedUser student = context.AccountManagementTestCaseContext.WithUser();

            Assert.ThrowsAsync<InnerLogicException>(() => context.GuildMemberService.UnblockStudent(user, guild.Id, student.Id));
        }

        [Test]
        public async Task AcceptRequest_RemoveFromRequestListAndAddToMembers()
        {
            TestCaseContext context = TestCaseContext.Case();
            AuthorizedUser user = context.AccountManagementTestCaseContext.WithUser();
            GuildProfileDto guild = context.GuildTestCaseContext.WithGuild(user);

            AuthorizedUser student = context.GuildTestCaseContext.WithGuildRequest(guild);

            await context.GuildMemberService.AcceptRequest(user, guild.Id, student.Id);
            GuildMemberLeaderBoardDto guildMemberLeaderBoardDto = await context.GuildService.GetGuildMemberLeaderBoard(guild.Id);
            GuildMemberImpactDto? member = guildMemberLeaderBoardDto.MembersImpact.Find(m => m.StudentInfoDto.Id == student.Id);
            GuildMember[] requests = await context.GuildMemberService.GetGuildRequests(user, guild.Id);

            Assert.IsNotNull(member);
            Assert.That(member.MemberType, Is.EqualTo(GuildMemberType.Member));
            Assert.That(requests.FirstOrDefault(m => m.MemberId == student.Id), Is.Null);
        }

        [Test]
        public void AcceptRequest_ForStudentWithoutRequest_ThrowsInnerLogicException()
        {
            TestCaseContext context = TestCaseContext.Case();
            AuthorizedUser user = context.AccountManagementTestCaseContext.WithUser();
            GuildProfileDto guild = context.GuildTestCaseContext.WithGuild(user);
            AuthorizedUser student = context.AccountManagementTestCaseContext.WithUser();

            Assert.ThrowsAsync<EntityNotFoundException>(() => context.GuildMemberService.AcceptRequest(user, guild.Id, student.Id));
        }

        [Test]
        public async Task RejectRequest_RemoveFromRequestList()
        {
            TestCaseContext context = TestCaseContext.Case();
            AuthorizedUser user = context.AccountManagementTestCaseContext.WithUser();
            GuildProfileDto guild = context.GuildTestCaseContext.WithGuild(user);
            AuthorizedUser student = context.GuildTestCaseContext.WithGuildRequest(guild);

            await context.GuildMemberService.RejectRequest(user, guild.Id, student.Id);
            GuildMemberLeaderBoardDto guildMemberLeaderBoardDto = await context.GuildService.GetGuildMemberLeaderBoard(guild.Id);
            GuildMemberImpactDto? member = guildMemberLeaderBoardDto.MembersImpact.Find(m => m.StudentInfoDto.Id == student.Id);
            GuildMember[] requests = await context.GuildMemberService.GetGuildRequests(user, guild.Id);

            Assert.That(member, Is.Null);
            Assert.That(requests.FirstOrDefault(m => m.MemberId == student.Id), Is.Null);
        }

        [Test]
        public void RejectRequest_ForStudentWithoutRequest_ThrowsInnerLogicException()
        {
            TestCaseContext context = TestCaseContext.Case();
            AuthorizedUser user = context.AccountManagementTestCaseContext.WithUser();
            GuildProfileDto guild = context.GuildTestCaseContext.WithGuild(user);

            AuthorizedUser student = context.AccountManagementTestCaseContext.WithUser();

            Assert.ThrowsAsync<InnerLogicException>(() => context.GuildMemberService.RejectRequest(user, guild.Id, student.Id));
        }

        [Test]
        public async Task UpdateGuild_UpdateHiringPolicyToClose_CloseGuild()
        {
            TestCaseContext context = TestCaseContext.Case();
            AuthorizedUser user = context.AccountManagementTestCaseContext.WithUser();
            GuildProfileDto guild = context.GuildTestCaseContext.WithGuild(user);
            AuthorizedUser student = context.GuildTestCaseContext.WithGuildRequest(guild);

            await context.GuildService.Update(user, GuildUpdateRequestDto.ForPolicyUpdate(guild.Id, GuildHiringPolicy.Close));

            GuildProfileDto guildEntity = await context.GuildService.Get(guild.Id);
            Assert.That(guildEntity.HiringPolicy, Is.EqualTo(GuildHiringPolicy.Close));
        }

        [Test]
        public async Task UpdateGuild_UpdateHiringPolicyToOpen_SwitchRequestsToMembers()
        {
            TestCaseContext context = TestCaseContext.Case();
            AuthorizedUser user = context.AccountManagementTestCaseContext.WithUser();
            GuildProfileDto guild = context.GuildTestCaseContext.WithGuild(user);
            AuthorizedUser student = context.GuildTestCaseContext.WithGuildRequest(guild);

            await context.GuildService.Update(user, GuildUpdateRequestDto.ForPolicyUpdate(guild.Id, GuildHiringPolicy.Close));
            await context.GuildService.Update(user, GuildUpdateRequestDto.ForPolicyUpdate(guild.Id, GuildHiringPolicy.Open));

            GuildMemberLeaderBoardDto guildMemberLeaderBoardDto = await context.GuildService.GetGuildMemberLeaderBoard(guild.Id);
            GuildMemberImpactDto? newMember = guildMemberLeaderBoardDto.MembersImpact.Find(m => m.StudentInfoDto.Id == student.Id);
            Assert.IsNotNull(newMember);
            Assert.That(newMember.MemberType, Is.EqualTo(GuildMemberType.Member));
        }

        [Test]
        public void GetGuildMemberLeaderBoard()
        {
            TestCaseContext context = TestCaseContext.Case();
            AuthorizedUser user = context.AccountManagementTestCaseContext.WithUser();
            GithubUser userData = context.GithubTestCaseContext.WithGithubAccount(user);

            GuildProfileDto guild = context.GuildTestCaseContext.WithGuild(user);
            GuildMemberLeaderBoardDto leaderboard = context.GuildService.GetGuildMemberLeaderBoard(guild.Id).Result;

            Assert.That(leaderboard.MembersImpact.Single().TotalRate,
                Is.EqualTo(userData.ContributionFullInfo?.Total ?? 0));
        }
    }
}
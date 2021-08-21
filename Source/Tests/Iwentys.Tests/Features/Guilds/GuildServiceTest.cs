using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Enums;
using Iwentys.Domain.Guilds.Models;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.DataAccess.Seeding.FakerEntities.Guilds;
using Iwentys.Tests.TestCaseContexts;
using NUnit.Framework;

namespace Iwentys.Tests.Features.Guilds
{
    [TestFixture]
    public class GuildServiceTest
    {
        [Test]
        public void CreateGuild_ShouldReturnCreatorAsMember()
        {
            TestCaseContext context = TestCaseContext.Case();
            IwentysUser user = context.AccountManagementTestCaseContext.WithIwentysUser();
            var guild = Guild.Create(user, null, GuildFaker.Instance.GetGuildCreateArguments());

            List<GuildMemberImpactDto> guildMemberImpactDtos = guild.GetImpact();

            bool isExist = guildMemberImpactDtos.Any(_ => _.StudentInfoDto.Id == user.Id);
            Assert.IsTrue(isExist);
        }

        [Test]
        public void CreateGuild_GuildStateIsPending()
        {
            TestCaseContext context = TestCaseContext.Case();
            IwentysUser user = context.AccountManagementTestCaseContext.WithIwentysUser();

            var guild = Guild.Create(user, null, GuildFaker.Instance.GetGuildCreateArguments());

            Assert.AreEqual(GuildType.Pending, guild.GuildType);
        }

        [Test]
        public void ApproveCreatedRepo_GuildStateIsCreated()
        {
            TestCaseContext context = TestCaseContext.Case();
            IwentysUser user = context.AccountManagementTestCaseContext.WithIwentysUser();
            IwentysUser admin = context.AccountManagementTestCaseContext.WithIwentysUser(true);
            var guild = Guild.Create(user, null, GuildFaker.Instance.GetGuildCreateArguments());

            guild.Approve(admin);

            Assert.AreEqual(GuildType.Created, guild.GuildType);
        }

        [Test]
        public void CreateTwoGuildForUser_ErrorUserAlreadyInGuild()
        {
            TestCaseContext context = TestCaseContext.Case();
            IwentysUser user = context.AccountManagementTestCaseContext.WithIwentysUser();
            var guild = Guild.Create(user, null, GuildFaker.Instance.GetGuildCreateArguments());

            Assert.Catch<InnerLogicException>(() => Guild.Create(user, guild, GuildFaker.Instance.GetGuildCreateArguments()));
        }

        [Test]
        public void GetGuildRequests_ForGuildEditorAndNoRequestsToGuild_EmptyGildMembersArray()
        {
            TestCaseContext context = TestCaseContext.Case();
            IwentysUser user = context.AccountManagementTestCaseContext.WithIwentysUser();
            var guild = Guild.Create(user, null, GuildFaker.Instance.GetGuildCreateArguments());

            List<GuildMember> requests = guild.Members.Where( m => m.MemberType == GuildMemberType.Requested).ToList();

            Assert.That(requests, Is.Not.Null);
            Assert.That(requests.Any(), Is.False);
        }

        [Test]
        public void GetGuildRequests_ForNotGuildMember_ThrowInnerLogicException()
        {
            TestCaseContext context = TestCaseContext.Case();
            IwentysUser user = context.AccountManagementTestCaseContext.WithIwentysUser();
            var guild = Guild.Create(user, null, GuildFaker.Instance.GetGuildCreateArguments());
            IwentysUser otherUser = context.AccountManagementTestCaseContext.WithIwentysUser();

            Assert.Throws<InnerLogicException>(() => guild.GetGuildRequests(otherUser));

        }

        [Test]
        public void GetGuildRequests_ForGuildMemberWithoutEditorPermissions_ThrowInnerLogicException()
        {
            TestCaseContext context = TestCaseContext.Case();
            IwentysUser user = context.AccountManagementTestCaseContext.WithIwentysUser();
            var guild = Guild.Create(user, null, GuildFaker.Instance.GetGuildCreateArguments());
            IwentysUser otherUser = context.AccountManagementTestCaseContext.WithIwentysUser();
            guild.Members.Add(new GuildMember(guild, otherUser, GuildMemberType.Member));

            Assert.Throws<InnerLogicException>(() => guild.GetGuildRequests(otherUser));
        }

        [Test]
        public void GetGuildRequests_ForGuildEditor_GildMembersArrayWithRequestStatus()
        {
            TestCaseContext context = TestCaseContext.Case();
            IwentysUser user = context.AccountManagementTestCaseContext.WithIwentysUser();
            IwentysUser member = context.AccountManagementTestCaseContext.WithIwentysUser();
            var guild = Guild.Create(user, null, GuildFaker.Instance.GetGuildCreateArguments());

            guild.HiringPolicy = GuildHiringPolicy.Close;
            guild.RequestEnterGuild(member, null, null);
            List<GuildMember> requests = guild.Members.Where(m => m.MemberType == GuildMemberType.Requested).ToList();

            Assert.That(requests, Is.Not.Null);
            Assert.That(requests.Length, Is.EqualTo(1));
            Assert.That(requests[0].MemberId, Is.EqualTo(member.Id));
            Assert.That(requests[0].MemberType, Is.EqualTo(GuildMemberType.Requested));
        }

        [Test]
        [Ignore("NRE")]
        public void BlockGuildMember_AddUserToBlockedListAndKickFromGuild()
        {
            TestCaseContext context = TestCaseContext.Case();
            IwentysUser user = context.AccountManagementTestCaseContext.WithIwentysUser();
            var guild = Guild.Create(user, null, GuildFaker.Instance.GetGuildCreateArguments());
            IwentysUser otherUser = context.AccountManagementTestCaseContext.WithIwentysUser();
            guild.Members.Add(new GuildMember(guild, otherUser, GuildMemberType.Member));

            guild.BlockMember(user, otherUser, new GuildLastLeave());
            List<GuildMember> blocked = guild.GetGuildBlocked(user);

            Assert.That(blocked.Find(m => m.MemberId == otherUser.Id), Is.Not.Null);
        }

        [Test]
        public void BlockGuildMember_BlockCreator_ThrowsInnerLogicException()
        {
            TestCaseContext context = TestCaseContext.Case();
            IwentysUser user = context.AccountManagementTestCaseContext.WithIwentysUser();
            var guild = Guild.Create(user, null, GuildFaker.Instance.GetGuildCreateArguments());
            IwentysUser otherUser = context.AccountManagementTestCaseContext.WithIwentysUser();
            guild.Members.Add(new GuildMember(guild, otherUser, GuildMemberType.Member));

            Assert.Throws<InnerLogicException>(() => guild.BlockMember(otherUser, user, new GuildLastLeave()));
        }

        [Test]
        public void BlockGuildMember_BlockNotGuildMember_ThrowsInnerLogicException()
        {
            TestCaseContext context = TestCaseContext.Case();
            IwentysUser user = context.AccountManagementTestCaseContext.WithIwentysUser();
            var guild = Guild.Create(user, null, GuildFaker.Instance.GetGuildCreateArguments());
            IwentysUser otherUser = context.AccountManagementTestCaseContext.WithIwentysUser();

            Assert.Throws<InnerLogicException>(() => guild.BlockMember(otherUser, user, new GuildLastLeave()));
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
        [Ignore("NRE")]
        public void UnblockStudent_RemoveStudentFromListOfBlocked()
        {
            TestCaseContext context = TestCaseContext.Case();
            IwentysUser user = context.AccountManagementTestCaseContext.WithIwentysUser();
            var guild = Guild.Create(user, null, GuildFaker.Instance.GetGuildCreateArguments());
            IwentysUser otherUser = context.AccountManagementTestCaseContext.WithIwentysUser();

            guild.BlockMember(user, otherUser, new GuildLastLeave());
            guild.UnblockStudent(user, otherUser, new GuildLastLeave());

            List<GuildMember> blocked = guild.GetGuildBlocked(user);

            Assert.That(blocked.Find(m => m.MemberId == otherUser.Id), Is.Not.Null);
        }

        [Test]
        public void UnblockStudent_NotBlockedInGuild_ThrowsInnerLogicException()
        {
            TestCaseContext context = TestCaseContext.Case();
            IwentysUser user = context.AccountManagementTestCaseContext.WithIwentysUser();
            var guild = Guild.Create(user, null, GuildFaker.Instance.GetGuildCreateArguments());
            IwentysUser otherUser = context.AccountManagementTestCaseContext.WithIwentysUser();

            Assert.Throws<InnerLogicException>(() => guild.UnblockStudent(user, otherUser, new GuildLastLeave()));
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
        public void RejectRequest_RemoveFromRequestList()
        {
            TestCaseContext context = TestCaseContext.Case();
            IwentysUser user = context.AccountManagementTestCaseContext.WithIwentysUser();
            var guild = Guild.Create(user, null, GuildFaker.Instance.GetGuildCreateArguments());
            IwentysUser student = context.AccountManagementTestCaseContext.WithIwentysUser();
            guild.Members.Add(new GuildMember(guild, student, GuildMemberType.Requested));
            guild.RejectUser(user, student, new GuildLastLeave());

            Assert.That(guild.Members.FirstOrDefault(m => m.MemberId == student.Id), Is.Null);
        }

        [Test]
        public void RejectRequest_ForStudentWithoutRequest_ThrowsInnerLogicException()
        {
            TestCaseContext context = TestCaseContext.Case();
            IwentysUser user = context.AccountManagementTestCaseContext.WithIwentysUser();
            var guild = Guild.Create(user, null, GuildFaker.Instance.GetGuildCreateArguments());

            IwentysUser student = context.AccountManagementTestCaseContext.WithIwentysUser();

            Assert.Throws<InnerLogicException>(() => guild.RejectUser(user, student, new GuildLastLeave()));
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
            IwentysUser user = context.AccountManagementTestCaseContext.WithIwentysUser();
            GithubUser userData = context.GithubTestCaseContext.WithGithubAccount(user);
            var guild = Guild.Create(user, null, GuildFaker.Instance.GetGuildCreateArguments());

            var leaderboard = new GuildMemberLeaderBoardDto(guild.GetImpact());

            Assert.That(leaderboard.MembersImpact.Single().TotalRate,
                Is.EqualTo(userData.ContributionFullInfo?.Total ?? 0));
        }
    }
}
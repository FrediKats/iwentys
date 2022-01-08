using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common;
using Iwentys.DataAccess.Seeding;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.Guilds;
using Iwentys.Tests.TestCaseContexts;
using NUnit.Framework;

namespace Iwentys.Tests.Features.Guilds
{
    [TestFixture]
    public class GuildServiceTest
    {
        //[Test]
        //public void CreateGuild_ShouldReturnCreatorAsMember()
        //{
        //    TestCaseContext context = TestCaseContext.Case();
        //    IwentysUser user = context.AccountManagementTestCaseContext.WithIwentysUser();
        //    var guild = Guild.Create(user, null, GuildFaker.Instance.GetGuildCreateArguments());

        //    List<GuildMemberImpactDto> guildMemberImpactDtos = guild.GetImpact();

        //    bool isExist = guildMemberImpactDtos.Any(_ => _.StudentInfoDto.Id == user.Id);
        //    Assert.IsTrue(isExist);
        //}

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

            guild.ApproveGuildCreation(admin);

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
            Assert.That(requests.Count, Is.EqualTo(1));
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
            IwentysUser user = context.AccountManagementTestCaseContext.WithIwentysUser();
            var guild = Guild.Create(user, null, GuildFaker.Instance.GetGuildCreateArguments());
            IwentysUser firstMentor = context.AccountManagementTestCaseContext.WithIwentysUser();
            IwentysUser secondMentor = context.AccountManagementTestCaseContext.WithIwentysUser();
            guild.Members.Add(new GuildMember(guild, firstMentor, GuildMemberType.Mentor));
            guild.Members.Add(new GuildMember(guild, secondMentor, GuildMemberType.Mentor));

            Assert.Throws<InnerLogicException>(() => guild.BlockMember(firstMentor, secondMentor, new GuildLastLeave()));
        }

        [Test]
        public void KickGuildMember_KickMentorByMentor_ThrowsInnerLogicException()
        {
            TestCaseContext context = TestCaseContext.Case();
            IwentysUser user = context.AccountManagementTestCaseContext.WithIwentysUser();
            var guild = Guild.Create(user, null, GuildFaker.Instance.GetGuildCreateArguments());
            IwentysUser firstMentor = context.AccountManagementTestCaseContext.WithIwentysUser();
            IwentysUser secondMentor = context.AccountManagementTestCaseContext.WithIwentysUser();
            guild.Members.Add(new GuildMember(guild, firstMentor, GuildMemberType.Mentor));
            guild.Members.Add(new GuildMember(guild, secondMentor, GuildMemberType.Mentor));

            Assert.Throws<InnerLogicException>(() => guild.RemoveMember(firstMentor, secondMentor, new GuildLastLeave()));
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
        public void AcceptRequest_RemoveFromRequestListAndAddToMembers()
        {
            TestCaseContext context = TestCaseContext.Case();
            IwentysUser user = context.AccountManagementTestCaseContext.WithIwentysUser();
            var guild = Guild.Create(user, null, GuildFaker.Instance.GetGuildCreateArguments());
            IwentysUser otherUser = context.AccountManagementTestCaseContext.WithIwentysUser();
            guild.Members.Add(new GuildMember(guild, otherUser, GuildMemberType.Requested));

            guild.ApproveEnterGuild(user, otherUser, new GuildLastLeave());
            var requests = guild.GetGuildRequests(user);
            Assert.That(requests.FirstOrDefault(m => m.MemberId == otherUser.Id), Is.Null);
        }

        [Test]
        public void AcceptRequest_ForStudentWithoutRequest_ThrowsInnerLogicException()
        {
            TestCaseContext context = TestCaseContext.Case();
            IwentysUser user = context.AccountManagementTestCaseContext.WithIwentysUser();
            var guild = Guild.Create(user, null, GuildFaker.Instance.GetGuildCreateArguments());
            IwentysUser otherUser = context.AccountManagementTestCaseContext.WithIwentysUser();

            Assert.Throws<EntityNotFoundException>(() => guild.ApproveEnterGuild(user, otherUser, new GuildLastLeave()));
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
        public void UpdateGuild_UpdateHiringPolicyToClose_CloseGuild()
        {
            TestCaseContext context = TestCaseContext.Case();
            IwentysUser user = context.AccountManagementTestCaseContext.WithIwentysUser();
            var guild = Guild.Create(user, null, GuildFaker.Instance.GetGuildCreateArguments());
            IwentysUser student = context.AccountManagementTestCaseContext.WithIwentysUser();
            guild.Members.Add(new GuildMember(guild, student, GuildMemberType.Requested));

            guild.Update(user, GuildUpdateRequestDto.ForPolicyUpdate(guild.Id, GuildHiringPolicy.Close));

            Assert.That(guild.HiringPolicy, Is.EqualTo(GuildHiringPolicy.Close));
        }

        [Test]
        public void UpdateGuild_UpdateHiringPolicyToOpen_SwitchRequestsToMembers()
        {
            TestCaseContext context = TestCaseContext.Case();
            IwentysUser user = context.AccountManagementTestCaseContext.WithIwentysUser();
            var guild = Guild.Create(user, null, GuildFaker.Instance.GetGuildCreateArguments());
            IwentysUser student = context.AccountManagementTestCaseContext.WithIwentysUser();

            guild.Update(user, GuildUpdateRequestDto.ForPolicyUpdate(guild.Id, GuildHiringPolicy.Close));
            guild.Members.Add(new GuildMember(guild, student, GuildMemberType.Requested));
            guild.Update(user, GuildUpdateRequestDto.ForPolicyUpdate(guild.Id, GuildHiringPolicy.Open));

            var newMember = guild.Members.FirstOrDefault(m => m.MemberId == student.Id);

            Assert.IsNotNull(newMember);
            Assert.That(newMember.MemberType, Is.EqualTo(GuildMemberType.Member));
        }

        //[Test]
        //public void GetGuildMemberLeaderBoard()
        //{
        //    TestCaseContext context = TestCaseContext.Case();
        //    IwentysUser user = context.AccountManagementTestCaseContext.WithIwentysUser();
        //    GithubUser userData = context.GithubTestCaseContext.WithGithubAccount(user);
        //    var guild = Guild.Create(user, null, GuildFaker.Instance.GetGuildCreateArguments());

        //    var leaderboard = new GuildMemberLeaderBoardDto(guild.GetImpact());

        //    Assert.That(leaderboard.MembersImpact.Single().TotalRate,
        //        Is.EqualTo(userData.ContributionFullInfo?.Total ?? 0));
        //}
    }
}
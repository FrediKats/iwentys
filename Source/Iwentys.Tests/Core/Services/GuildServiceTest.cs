﻿using System.Collections.Generic;
using System.Linq;
using Iwentys.Core.DomainModel;
using Iwentys.Database.Repositories;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Github;
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

            Assert.IsTrue(guild.MemberLeaderBoard.Members.Any(m => m.Id == user.Id));
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
                .WithMentor(guild, admin, out AuthorizedUser mentor)
                .WithStudentProject(user, out StudentProject project)
                .WithTribute(user, project, out Tribute _);

            Tribute[] tributes = context.GuildService.GetPendingTributes(mentor);
            
            Assert.IsNotEmpty(tributes);
            Assert.True(tributes.Any(t => t.ProjectId == project.Id));
        }

        [Test]
        public void GetGuildRequests_ForGuildEditorAndNoRequestsToGuild_EmptyGildMembersArray()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild);

            List<GuildMember> requests = context.GuildService.GetGuildRequests(user, guild.Id).ToList();

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

            Assert.Throws<InnerLogicException>(() => context.GuildService.GetGuildRequests(student, guild.Id));
        }

        [Test]
        public void GetGuildRequests_ForGuildMemberWithoutEditorPermissions_ThrowInnerLogicException()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild)
                .WithGuildMember(guild, out AuthorizedUser member);

            Assert.Throws<InnerLogicException>(() => context.GuildService.GetGuildRequests(member, guild.Id));
        }

        [Test]
        public void GetGuildRequests_ForGuildEditor_GildMembersArrayWithRequestStatus()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild)
                .WithGuildRequest(guild, out AuthorizedUser student);

            List<GuildMember> requests = context.GuildService.GetGuildRequests(user, guild.Id).ToList();

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

            List<GuildMember> blocked = context.GuildService.GetGuildBlocked(user, guild.Id).ToList();

            Assert.That(blocked, Is.Not.Null);
            Assert.That(blocked.Length, Is.EqualTo(1));
            Assert.That(blocked[0].MemberId, Is.EqualTo(student.Id));
            Assert.That(blocked[0].MemberType, Is.EqualTo(GuildMemberType.Blocked));
        }

        [Test]
        public void BlockGuildMember_AddUserToBlockedListAndKickFromGuild()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild)
                .WithGuildMember(guild, out AuthorizedUser member);

            context.GuildService.BlockGuildMember(user, guild.Id, member.Id);
            List<GuildMember> blocked = context.GuildService.GetGuildBlocked(user, guild.Id).ToList();
            Guild memberGuild = context.GuildRepository.ReadForStudent(member.Id);

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

            Assert.Throws<InnerLogicException>(() =>  context.GuildService.BlockGuildMember(user, guild.Id, user.Id));
        }

        [Test]
        public void BlockGuildMember_BlockNotGuildMember_ThrowsInnerLogicException()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild)
                .WithNewStudent(out AuthorizedUser student);

            Assert.Throws<InnerLogicException>(() =>  context.GuildService.BlockGuildMember(user, guild.Id, student.Id));
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

            Assert.Throws<InnerLogicException>(() =>  context.GuildService.BlockGuildMember(mentor, guild.Id, anotherMentor.Id));
        }

        [Test]
        public void KickGuildMember_KickMemberFromGuild()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild)
                .WithGuildMember(guild, out AuthorizedUser member);

            context.GuildService.KickGuildMember(user, guild.Id, member.Id);
            Guild memberGuild = context.GuildRepository.ReadForStudent(member.Id);

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

            Assert.Throws<InnerLogicException>(() =>  context.GuildService.KickGuildMember(mentor, guild.Id, anotherMentor.Id));
        }

        [Test]
        public void UnblockStudent_RemoveStudentFromListOfBlocked()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild)
                .WithGuildBlocked(guild, out AuthorizedUser student);

            context.GuildService.UnblockStudent(user, guild.Id, student.Id);
            List<GuildMember> blocked = context.GuildService.GetGuildBlocked(user, guild.Id).ToList();


            Assert.That(blocked.Find(m => m.MemberId == student.Id), Is.Null);
        }

        [Test]
        public void UnblockStudent_NotBlockedInGuild_ThrowsInnerLogicException()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild)
                .WithNewStudent(out AuthorizedUser student);

            Assert.Throws<InnerLogicException>(() => context.GuildService.UnblockStudent(user, guild.Id, student.Id));
        }

        [Test]
        public void AcceptRequest_RemoveFromRequestListAndAddToMembers()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild)
                .WithGuildRequest(guild, out AuthorizedUser student);

            context.GuildService.AcceptRequest(user, guild.Id, student.Id);
            GuildMember member = context.GuildRepository.Get(guild.Id).Members.Find(m => m.MemberId == student.Id);
            List<GuildMember> requests = context.GuildService.GetGuildRequests(user, guild.Id).ToList();

            Assert.That(member.MemberType, Is.EqualTo(GuildMemberType.Member));
            Assert.That(requests.Find(m => m.MemberId == student.Id), Is.Null);
        }

        [Test]
        public void AcceptRequest_ForStudentWithoutRequest_ThrowsInnerLogicException()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild)
                .WithNewStudent(out AuthorizedUser student);

            Assert.Throws<InnerLogicException>(() => context.GuildService.AcceptRequest(user, guild.Id, student.Id));
        }

        [Test]
        public void RejectRequest_RemoveFromRequestList()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild)
                .WithGuildRequest(guild, out AuthorizedUser student);

            context.GuildService.RejectRequest(user, guild.Id, student.Id);
            GuildMember member = context.GuildRepository.Get(guild.Id).Members.Find(m => m.MemberId == student.Id);
            List<GuildMember> requests = context.GuildService.GetGuildRequests(user, guild.Id).ToList();

            Assert.That(member, Is.Null);
            Assert.That(requests.Find(m => m.MemberId == student.Id), Is.Null);
        }

        [Test]
        public void RejectRequest_ForStudentWithoutRequest_ThrowsInnerLogicException()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild)
                .WithNewStudent(out AuthorizedUser student);

            Assert.Throws<InnerLogicException>(() => context.GuildService.RejectRequest(user, guild.Id, student.Id));
        }

        [Test]
        public void UpdateGuild_UpdateHiringPolicyToClose_CloseGuild()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild)
                .WithGuildRequest(guild, out AuthorizedUser student);
            context.GuildService.Update(user, new GuildUpdateArgumentDto() {Id = guild.Id, HiringPolicy = GuildHiringPolicy.Close});

            Assert.That(context.GuildRepository.Get(guild.Id).HiringPolicy, Is.EqualTo(GuildHiringPolicy.Close));
        }

        [Test]
        public void UpdateGuild_UpdateHiringPolicyToOpen_SwitchRequestsToMembers()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGuild(user, out GuildProfileDto guild)
                .WithGuildRequest(guild, out AuthorizedUser student);
            context.GuildService.Update(user, new GuildUpdateArgumentDto() {Id = guild.Id, HiringPolicy = GuildHiringPolicy.Close});
            context.GuildService.Update(user, new GuildUpdateArgumentDto() {Id = guild.Id, HiringPolicy = GuildHiringPolicy.Open});

            Assert.That(context.GuildRepository.Get(guild.Id).Members.ToList().Find(m => m.MemberId == student.Id).MemberType,
                Is.EqualTo(GuildMemberType.Member));
        }

        [Test]

        public void GetGuildMemberLeaderBoard()
        {
            var context = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithGithubUser("test", out GithubUser ghUser)
                .WithGithubRepository(user, ghUser, out GithubUserData userData)
                .WithGuild(user, out GuildProfileDto guild);

            Assert.That(context.GuildService.GetGuildMemberLeaderBoard(guild.Id).MembersImpact.Single().TotalRate,
                Is.EqualTo(userData.ContributionFullInfo.Total));
        }
    }
}
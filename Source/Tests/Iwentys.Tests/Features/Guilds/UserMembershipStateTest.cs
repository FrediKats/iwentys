using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.GithubIntegration.Models;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Enums;
using Iwentys.Infrastructure.Application.Controllers.GithubIntegration;
using Moq;
using NUnit.Framework;

namespace Iwentys.Tests.Features.Guilds
{
    [TestFixture]
    public class UserMembershipStateTest
    {
        // User without guild
        //      is not in blocked list
        //      have not send request to this guild
        //      left guild later than 24 hours before try to enter
        //
        // Guild is open
        [SetUp]
        public void SetUp()
        {
            _student = new IwentysUser
            {
                Id = 1,
                LastOnlineTime = DateTime.MinValue.ToUniversalTime(),
                GithubUsername = string.Empty
            };

            _guild = new Guild
            {
                Id = 1,
                Members = new List<GuildMember>
                {
                    new GuildMember(1, 1, GuildMemberType.Creator)
                },
                HiringPolicy = GuildHiringPolicy.Open,
                PinnedProjects = new List<GuildPinnedProject>()
            };

            //_tributeRepository = new Mock<IGenericRepository<Tribute>>();
            //_tributeRepository
            //    .Setup(r => r.ReadStudentActiveTribute(It.IsAny<Int32>(), It.IsAny<Int32>()))
            //    .Returns(default(Tribute));

            _githubUserDataService = new Mock<GithubIntegrationService>();
            //_githubUserDataService
            //    .Setup(a => a.GetRepository(It.IsAny<String>(), It.IsAny<String>()))
            //    .Returns(default(GithubRepositoryInfoDto));
            _githubUserDataService
                .Setup(a => a.User.GetGithubUser(It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new GithubUser {ContributionFullInfo = new ContributionFullInfo {RawActivity = new CodingActivityInfo {Contributions = new List<ContributionsInfo>(), Years = new List<YearActivityInfo>()}}}));

            //_guildRepository = new Mock<GuildRepository>();
            //_guildRepository
            //    .Setup(r => r.ReadForStudent(It.IsAny<Int32>()))
            //    .Returns(default(Guild));

            //_guildMemberRepository = new Mock<GuildMemberRepository>();
            //_guildMemberRepository
            //    .Setup(r => r.IsStudentHaveRequest(It.IsAny<Int32>()))
            //    .Returns(false);
        }

        private Guild _guild;

        private IwentysUser _student;

        private Mock<GithubIntegrationService> _githubUserDataService;

        [Test]
        [Ignore("NSE")]
        public void GetGuild_ForUserWithNoGuildAndForOpenedGuild_UserMembershipStateIsCanEnter()
        {
            //IwentysUser user = await _guildDomain._userRepository.GetById(_student.Id);
            //Guild userGuild = _guildDomain._guildMemberRepositoryNew.ReadForStudent(user.Id);
            //Assert.That((await _guildDomain.GetUserMembershipState(user, userGuild)).Result, Is.EqualTo(UserMembershipState.CanEnter));
        }

        [Test]
        [Ignore("NSE")]
        public void GetGuild_ForUserWithNoGuildAndForClosedGuild_UserMembershipStateIsCanRequest()
        {
            _guild.HiringPolicy = GuildHiringPolicy.Close;

            //IwentysUser user = await _guildDomain._userRepository.GetById(_student.Id);
            //Guild userGuild = _guildDomain._guildMemberRepositoryNew.ReadForStudent(user.Id);
            //Assert.That((await _guildDomain.GetUserMembershipState(user, userGuild)).Result, Is.EqualTo(UserMembershipState.CanRequest));
        }

        [Test]
        [Ignore("NRE")]
        public void GetGuild_ForGuildMember_UserMembershipStateIsEntered()
        {
            _guild.Members.Add(new GuildMember(_guild, _student, GuildMemberType.Member));
            //_guildRepository
            //    .Setup(r => r.ReadForStudent(_student.Id))
            //    .Returns(_guild);


            //IwentysUser user = await _guildDomain._userRepository.GetById(1);
            //Guild userGuild = _guildDomain._guildMemberRepositoryNew.ReadForStudent(user.Id);
            //Assert.That((await _guildDomain.GetUserMembershipState(user, userGuild)).Result, Is.EqualTo(UserMembershipState.Entered));
        }

        [Test]
        [Ignore("NRE")]
        public void GetGuild_ForBlockedUser_UserMembershipStateIsBlocked()
        {
            _guild.Members.Add(new GuildMember(_guild, _student, GuildMemberType.Blocked));

            //IwentysUser user = await _guildDomain._userRepository.GetById(1);
            //Guild userGuild = _guildDomain._guildMemberRepositoryNew.ReadForStudent(user.Id);
            //Assert.That((await _guildDomain.GetUserMembershipState(user, userGuild)).Result, Is.EqualTo(UserMembershipState.Blocked));
        }

        [Test]
        [Ignore("NEE")]
        public void GetGuild_ForUserInAnotherGuild_UserMembershipStateIsBlocked()
        {
            //_guildRepository
            //    .Setup(r => r.ReadForStudent(_student.Id))
            //    .Returns(new Guild() { Id = 2 });

            //IwentysUser user = await _guildDomain._userRepository.GetById(1);
            //Guild userGuild = _guildDomain._guildMemberRepositoryNew.ReadForStudent(user.Id);
            //Assert.That((await _guildDomain.GetUserMembershipState(user, userGuild)).Result, Is.EqualTo(UserMembershipState.Blocked));
        }

        [Test]
        [Ignore("NRE")]
        public void GetGuild_ForUserWithRequestToThisGuild_UserMembershipStateIsRequested()
        {
            _guild.Members.Add(new GuildMember(_guild, _student, GuildMemberType.Requested));
            //_guildMemberRepository
            //    .Setup(r => r.IsStudentHaveRequest(_student.Id))
            //    .Returns(true);

            //IwentysUser user = await _guildDomain._userRepository.GetById(1);
            //Guild userGuild = _guildDomain._guildMemberRepositoryNew.ReadForStudent(user.Id);
            //Assert.That((await _guildDomain.GetUserMembershipState(user, userGuild)).Result, Is.EqualTo(UserMembershipState.Requested));
        }

        [Test]
        [Ignore("NSE")]
        public void GetGuild_ForUserWithRequestToAnotherGuild_UserMembershipStateIsBlocked()
        {
            //_guildMemberRepository
            //    .Setup(r => r.IsStudentHaveRequest(_student.Id))
            //    .Returns(true);

            //IwentysUser user = await _guildDomain._userRepository.GetById(1);
            //Guild userGuild = _guildDomain._guildMemberRepositoryNew.ReadForStudent(user.Id);
            //Assert.That((await _guildDomain.GetUserMembershipState(user, userGuild)).Result, Is.EqualTo(UserMembershipState.Blocked));
        }

        [Test]
        [Ignore("NSE")]
        public void GetGuild_ForUserWhichLeftGuild23hoursAgo_UserMembershipStateIsBlocked()
        {
            //_student.GuildLeftTime = DateTime.UtcNow.AddHours(-23);

            //IwentysUser user = await _guildDomain._userRepository.GetById(1);
            //Guild userGuild = _guildDomain._guildMemberRepositoryNew.ReadForStudent(user.Id);
            //Assert.That((await _guildDomain.GetUserMembershipState(user, userGuild)).Result, Is.EqualTo(UserMembershipState.Blocked));
        }
    }
}
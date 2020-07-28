using System;
using System.Collections.Generic;
using Iwentys.Core.DomainModel;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Transferable;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Transferable.Voting;
using Iwentys.Models.Types;
using Iwentys.Models.Types.Github;
using Iwentys.Models.Types.Guilds;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuildController : ControllerBase
    {
        private readonly IGuildService _guildService;

        public GuildController(IGuildService guildService)
        {
            _guildService = guildService;
        }

        [HttpPost]
        public GuildProfileShortInfoDto Create([FromBody] GuildCreateArgumentDto arguments)
        {
            AuthorizedUser creator = AuthorizedUser.DebugAuth();
            return _guildService.Create(creator, arguments);
        }

        [HttpPost("update")]
        public GuildProfileShortInfoDto Update([FromBody] GuildUpdateArgumentDto arguments)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return _guildService.Update(user, arguments);
        }

        [HttpGet]
        public IEnumerable<GuildProfileDto> Get()
        {
            return _guildService.Get();
        }

        [HttpGet("{id}")]
        public GuildProfileDto Get(int id)
        {
            var students = new List<Student>
            {
                new Student()
                {
                    Id = 1,
                    FirstName = "Fredi",
                    MiddleName = "String",
                    SecondName = "Kats",
                    Role = UserType.Common,
                    Group = "M3XXX",
                    GithubUsername = "InRedikaWB",
                    CreationTime = DateTime.Now,
                    LastOnlineTime = DateTime.Now,
                    BarsPoints = Int16.MaxValue
                },
                new Student()
                {
                    Id = 1,
                    FirstName = "Jon",
                    MiddleName = String.Empty,
                    SecondName = "Skeet",
                    Role = UserType.Common,
                    Group = "M3XXX",
                    GithubUsername = "jskeet",
                    CreationTime = DateTime.Now,
                    LastOnlineTime = DateTime.Now,
                    BarsPoints = 0
                }
            };
            var leaderBoard = new GuildMemberLeaderBoard
            {
                TotalRate = 100,
                Members = students,
                MembersImpact = new List<(String Username, Int32 TotalRate)>
                {
                    ("InRedikaWB", 70),
                    ("jskeet", 30)
                }
            };
            var achievements = new List<AchievementInfoDto>
            {
                new AchievementInfoDto
                {
                    Url = "#",
                    Name = "The first!",
                    Description = "The first guild in university."
                },
                new AchievementInfoDto
                {
                    Url = "#",
                    Name = "The best!",
                    Description = "The best guild in university."
                }

            };
            var repositories = new List<GithubRepository>
            {
                new GithubRepository(1, "Main","Место, где будет хранится основная информация связанная с жизнью TEF", "https://github.com/TEF-Dev/Main", 0),
                new GithubRepository(2, "Recademy",String.Empty, "https://github.com/TEF-Dev/Recademy", 3),
            };
            return new GuildProfileDto()
            {
                Id = 1,
                Title = "TEF",
                Bio = "Best ITMO C# community!",
                LogoUrl = "https://sun9-58.userapi.com/AbGPM3TA6R82X3Jj2F-GY2d-NrzFAgC0_fmkiA/XlxgCXVtyiM.jpg",
                HiringPolicy = GuildHiringPolicy.Open,

                Leader = students[0],
                Totem = students[1],

                MemberLeaderBoard = leaderBoard,
                Achievements = achievements,
                PinnedRepositories = repositories
            };

            //AuthorizedUser user = AuthorizedUser.DebugAuth();
            //return _guildService.Get(id, user.Profile.Id);
        }

        [HttpPut("{guildId}/enter")]
        public GuildProfileDto Enter(int guildId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return _guildService.EnterGuild(user, guildId);
        }

        [HttpPut("{guildId}/request")]
        public GuildProfileDto SendRequest(int guildId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return _guildService.RequestGuild(user, guildId);
        }

        [HttpPut("{guildId}/leave")]
        public GuildProfileDto Leave(int guildId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return _guildService.LeaveGuild(user, guildId);
        }

        [HttpGet("{guildId}/requests")]
        public GuildMember[] GetGuildRequests(int guildId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return _guildService.GetGuildRequests(user, guildId);
        }

        [HttpPost("{guildId}/VotingForLeader")]
        public void VotingForLeader(int guildId, [FromBody] GuildLeaderVotingCreateDto guildLeaderVotingCreateDto)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.StartVotingForLeader(user, guildId, guildLeaderVotingCreateDto);
        }

        [HttpPost("{guildId}/VotingForTotem")]
        public void VotingForTotem(int guildId, [FromBody] GuildTotemVotingCreateDto guildTotemVotingCreateDto)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.StartVotingForTotem(user, guildId, guildTotemVotingCreateDto);
        }

        [HttpPost("{guildId}/SetTotem")]
        public void SetTotem(int guildId, [FromBody] int totemId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.SetTotem(user, guildId, totemId);
        }

        [HttpPost("{guildId}/pinned")]
        public GithubRepository AddPinnedProject(int guildId, [FromBody] string repositoryUrl)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return _guildService.AddPinnedRepository(user, guildId, repositoryUrl);
        }

        [HttpDelete("{guildId}/pinned")]
        public GithubRepository DeletePinnedProject(int guildId, [FromBody] string repositoryUrl)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return _guildService.DeletePinnedRepository(user, guildId, repositoryUrl);
        }
    }
}
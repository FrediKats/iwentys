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
        public IEnumerable<GuildProfilePreviewDto> GetOverview([FromQuery]int skip = 0, [FromQuery]int take = 20)
        {
            return _guildService.GetOverview(skip, take);
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
                    CreationTime = DateTime.UtcNow,
                    LastOnlineTime = DateTime.UtcNow,
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
                    CreationTime = DateTime.UtcNow,
                    LastOnlineTime = DateTime.UtcNow,
                    BarsPoints = 0
                }
            };
            var leaderBoard = new GuildMemberLeaderBoard
            {
                TotalRate = 100,
                Members = students,
                MembersImpact = new List<GuildMemberImpact>
                {
                    new GuildMemberImpact("InRedikaWB", 70),
                    new GuildMemberImpact("jskeet", 30),
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
                PinnedRepositories = repositories,
                UserMembershipState = UserMembershipState.CanEnter
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

        [HttpGet("{guildId}/request")]
        public GuildMember[] GetGuildRequests(int guildId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return _guildService.GetGuildRequests(user, guildId);
        }

        [HttpGet("{guildId}/blocked")]
        public GuildMember[] GetGuildBlocked(int guildId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return _guildService.GetGuildBlocked(user, guildId);
        }

        [HttpPut("{guildId}/member/{memberId}/block")]
        public void BlockGuildMember(int guildId, int memberId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.BlockGuildMember(user, guildId, memberId);
        }

        [HttpPut("{guildId}/blocked/{studentId}/unblock")]
        public void UnblockGuildMember(int guildId, int studentId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.UnblockStudent(user, guildId, studentId);
        }

        [HttpPut("{guildId}/member/{memberId}/kick")]
        public void KickGuildMember(int guildId, int memberId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.KickGuildMember(user, guildId, memberId);
        }

        [HttpPut("{guildId}/request/{studentId}/accept")]
        public void AcceptRequest(int guildId, int studentId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.AcceptRequest(user, guildId, studentId);
        }

        [HttpPut("{guildId}/request/{studentId}/reject")]
        public void RejectRequest(int guildId, int studentId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            _guildService.RejectRequest(user, guildId, studentId);
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
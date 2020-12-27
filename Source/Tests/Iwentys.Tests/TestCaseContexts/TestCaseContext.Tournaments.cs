using System;
using Iwentys.Features.Guilds.Models;
using Iwentys.Features.Guilds.Tournaments.Enums;
using Iwentys.Features.Guilds.Tournaments.Models;
using Iwentys.Features.Guilds.Tributes.Models;
using Iwentys.Features.Students.Domain;

namespace Iwentys.Tests.TestCaseContexts
{
    public partial class TestCaseContext
    {
        public TestCaseContext WithCodeMarathon(AuthorizedUser userInfo, out TournamentInfoResponse tournament)
        {
            var createCodeMarathonTournamentArguments = new CreateCodeMarathonTournamentArguments()
            {
                ActivityType = CodeMarathonAllowedActivityType.All,
                MembersType = CodeMarathonAllowedMembersType.All,
                StartTime = DateTime.UtcNow.AddHours(-1),
                EndTime = DateTime.UtcNow.AddHours(1),
                Name = "Test tournament"
            };
            tournament = TournamentService.CreateCodeMarathon(userInfo, createCodeMarathonTournamentArguments).Result;
            return this;
        }
    }
}
using System;
using Bogus;
using Iwentys.Domain.Guilds;

namespace Iwentys.DataAccess.Seeding;

public class TournamentFaker
{
    public static readonly TournamentFaker Instance = new TournamentFaker();

    private readonly Faker _faker = new Faker();

    public CreateCodeMarathonTournamentArguments NewCodeMarathon()
    {
        return new CreateCodeMarathonTournamentArguments
        {
            ActivityType = CodeMarathonAllowedActivityType.All,
            MembersType = CodeMarathonAllowedMembersType.All,
            StartTime = DateTime.UtcNow.AddHours(-1),
            EndTime = DateTime.UtcNow.AddHours(1),
            Name = _faker.Lorem.Word()
        };
    }
}
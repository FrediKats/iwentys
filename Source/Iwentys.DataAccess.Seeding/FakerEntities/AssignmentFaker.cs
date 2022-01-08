using System;
using Bogus;
using Iwentys.Domain.Assignments;

namespace Iwentys.DataAccess.Seeding;

public class AssignmentFaker
{
    public static readonly AssignmentFaker Instance = new AssignmentFaker();

    private readonly Faker _faker = new Faker();

    public AssignmentCreateArguments CreateAssignmentCreateArguments()
    {
        return new AssignmentCreateArguments
        {
            Title = _faker.Hacker.IngVerb(),
            Description = _faker.Lorem.Paragraph(),
            DeadlineTimeUtc = DateTime.UtcNow.AddDays(1),
            ForStudyGroup = false
        };
    }
}
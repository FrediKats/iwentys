using System;
using Bogus;
using Iwentys.Features.Assignments.Models;

namespace Iwentys.Database.Seeding.FakerEntities
{
    public class AssignmentCreateRequestFaker : Faker<AssignmentCreateArguments>
    {
        public AssignmentCreateRequestFaker()
        {
            RuleFor(a => a.Title, f => f.Hacker.IngVerb())
                .RuleFor(a => a.Description, f => f.Hacker.IngVerb())
                .RuleFor(a => a.SubjectId, _ => null)
                .RuleFor(a => a.Deadline, DateTime.UtcNow.AddDays(1))
                .RuleFor(a => a.ForStudyGroup, false);
        }
    }
}
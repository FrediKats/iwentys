using System;
using Bogus;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Enums;

namespace Iwentys.Database.Seeding.FakerEntities
{
    public class StudentFaker : Faker<Student>
    {
        public StudentFaker(Func<int> groupIdSelector)
        {
            RuleFor(s => s.Id, f => f.IndexFaker + 1)
                .RuleFor(s => s.FirstName, f => f.Name.FirstName())
                .RuleFor(s => s.SecondName, f => f.Name.LastName())
                .RuleFor(s => s.Role, StudentRole.Common)
                .RuleFor(s => s.Type, StudentType.Budgetary)
                .RuleFor(s => s.CreationTime, DateTime.UtcNow)
                .RuleFor(s => s.LastOnlineTime, DateTime.UtcNow)
                .RuleFor(s => s.GroupId, _ => groupIdSelector.Invoke())
                .RuleFor(s => s.AvatarUrl, f => f.Image.PicsumUrl())
                .RuleFor(s => s.GithubUsername, f => f.Person.UserName);
        }
    }
}
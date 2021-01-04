using System;
using Bogus;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Enums;

namespace Iwentys.Database.Seeding.FakerEntities
{
    public class StudentFaker : Faker<Student>
    {
        public StudentFaker()
        {
            RuleFor(s => s.Id, f => UniversitySystemUserFaker.Instance.GetIdentifier())
                .RuleFor(s => s.FirstName, f => f.Name.FirstName())
                .RuleFor(s => s.SecondName, f => f.Name.LastName())
                .RuleFor(s => s.Type, StudentType.Budgetary)
                .RuleFor(s => s.CreationTime, DateTime.UtcNow)
                .RuleFor(s => s.LastOnlineTime, DateTime.UtcNow)
                .RuleFor(s => s.AvatarUrl, f => f.Image.PicsumUrl())
                .RuleFor(s => s.GithubUsername, f => f.Person.UserName);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Iwentys.Database.Seeding.Tools;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Enums;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class StudentGenerator
    {
        private const int StudentCount = 200;

        public StudentGenerator(List<StudyGroupEntity> studyGroups)
        {
            Faker = new Faker<StudentEntity>()
                .RuleFor(s => s.Id, f => f.IndexFaker++ + 1)
                .RuleFor(s => s.FirstName, f => f.Name.FirstName())
                .RuleFor(s => s.SecondName, f => f.Name.LastName())
                .RuleFor(s => s.Role, StudentRole.Common)
                .RuleFor(s => s.Type, StudentType.Budgetary)
                .RuleFor(s => s.CreationTime, DateTime.UtcNow)
                .RuleFor(s => s.LastOnlineTime, DateTime.UtcNow)
                .RuleFor(s => s.GroupId, _ => studyGroups.GetRandom().Id)
                .RuleFor(s => s.AvatarUrl, f => f.Image.PicsumUrl())
                .RuleFor(s => s.GithubUsername, f => f.Person.UserName);

            Students = Faker.Generate(StudentCount);
            Students.Add(new StudentEntity
            {
                Id = 228617,
                FirstName = "Фреди",
                MiddleName = "Кисикович",
                SecondName = "Катс",
                Role = StudentRole.Admin,
                GroupId = studyGroups.First(g => g.GroupName.Contains("3505")).Id,
                GithubUsername = "InRedikaWB",
                CreationTime = DateTime.UtcNow,
                LastOnlineTime = DateTime.UtcNow,
                BarsPoints = short.MaxValue,
                AvatarUrl = new Faker().Image.PicsumUrl()
            });

            Students
                .GroupBy(s => s.GroupId)
                .Select(g => g.FirstOrDefault())
                .Where(s => s is not null)
                .ToList()
                .ForEach(s => s.Role = StudentRole.GroupAdmin);
        }

        public Faker<StudentEntity> Faker { get; }
        public List<StudentEntity> Students { get; set; }
    }
}
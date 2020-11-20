using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Iwentys.Database.Seeding.Tools;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Types;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class StudentGenerator
    {
        private const int StudentCount = 100;

        private readonly List<StudyGroupEntity> _studyGroups;

        public StudentGenerator(List<StudyGroupEntity> studyGroups)
        {
            _studyGroups = studyGroups;

            Faker = new Faker<StudentEntity>()
                .RuleFor(s => s.Id, f => f.IndexFaker++ + 1)
                .RuleFor(s => s.FirstName, f => f.Name.FirstName())
                .RuleFor(s => s.SecondName, f => f.Name.LastName())
                .RuleFor(s => s.Role, UserType.Common)
                .RuleFor(s => s.Type, StudentType.Budgetary)
                .RuleFor(s => s.CreationTime, DateTime.UtcNow)
                .RuleFor(s => s.LastOnlineTime, DateTime.UtcNow)
                .RuleFor(s => s.GroupId, _ => _studyGroups.GetRandom().Id);

            Students = Faker.Generate(StudentCount);
            Students.Add(new StudentEntity
            {
                Id = 228617,
                FirstName = "Фреди",
                MiddleName = "Кисикович",
                SecondName = "Катс",
                Role = UserType.Admin,
                GroupId = _studyGroups.First(g => g.GroupName.Contains("3505")).Id,
                GithubUsername = "InRedikaWB",
                CreationTime = DateTime.UtcNow,
                LastOnlineTime = DateTime.UtcNow,
                BarsPoints = short.MaxValue
            });
        }

        public Faker<StudentEntity> Faker { get; }
        public List<StudentEntity> Students { get; set; }
    }
}
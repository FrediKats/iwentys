using System.Collections.Generic;
using Bogus;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Types;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class GuildGenerator
    {
        private const int GuildCount = 5;

        public List<GuildEntity> Guilds;
        private List<StudentEntity> _students;
        public GuildGenerator(List<StudentEntity> students)
        {
            _students = students;
            var faker = new Faker<GuildEntity>()
                .RuleFor(g => g.Id, f => f.IndexFaker)
                .RuleFor(g => g.Title, f => f.Company.CompanyName())
                .RuleFor(g => g.Bio, f => f.Lorem.Paragraph())
                .RuleFor(g => g.LogoUrl, f => f.Image.PicsumUrl())
                .RuleFor(g => g.HiringPolicy, GuildHiringPolicy.Open)
                .RuleFor(g => g.GuildType, GuildType.Created);

            Guilds = faker.Generate(GuildCount);
        }
    }
}
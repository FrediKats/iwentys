using System.Collections.Generic;
using System.Linq;
using Bogus;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Types;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class GuildGenerator
    {
        private const int GuildCount = 5;

        public List<GuildEntity> Guilds { get; }
        public List<GuildMemberEntity> GuildMembers { get; }

        private List<StudentEntity> _students;

        public GuildGenerator(List<StudentEntity> students)
        {
            _students = students;
            var faker = new Faker<GuildEntity>()
                .RuleFor(g => g.Id, f => f.IndexVariable++ + 1)
                .RuleFor(g => g.Title, f => f.Company.CompanyName())
                .RuleFor(g => g.Bio, f => f.Lorem.Paragraph())
                .RuleFor(g => g.LogoUrl, f => f.Image.PicsumUrl())
                .RuleFor(g => g.HiringPolicy, GuildHiringPolicy.Open)
                .RuleFor(g => g.GuildType, GuildType.Created);

            Guilds = faker.Generate(GuildCount);
            GuildMembers = new List<GuildMemberEntity>();

            int usedCount = 0;
            foreach (GuildEntity guild in Guilds)
            {
                var creator = new GuildMemberEntity(guild, _students[usedCount], GuildMemberType.Creator);
                GuildMembers.Add(creator);
                usedCount++;
                
                List<GuildMemberEntity> members = _students
                    .Skip(usedCount)
                    .Take(10)
                    .Select(s => new GuildMemberEntity(guild, s, GuildMemberType.Member))
                    .ToList();
                GuildMembers.AddRange(members);
                usedCount += 10;
            }

            //TODO: add guild pinned project
        }
    }
}
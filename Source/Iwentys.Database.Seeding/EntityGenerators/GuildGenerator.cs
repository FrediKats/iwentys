using System.Collections.Generic;
using System.Linq;
using Bogus;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.StudentFeature.Entities;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class GuildGenerator
    {
        private const int GuildCount = 5;

        public List<GuildEntity> Guilds { get; }
        public List<GuildMemberEntity> GuildMembers { get; }

        public GuildGenerator(List<StudentEntity> students)
        {
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
                var creator = new GuildMemberEntity(guild, students[usedCount], GuildMemberType.Creator);
                GuildMembers.Add(creator);
                usedCount++;
                
                List<GuildMemberEntity> members = students
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
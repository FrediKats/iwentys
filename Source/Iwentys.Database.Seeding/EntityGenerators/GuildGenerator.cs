using System.Collections.Generic;
using System.Linq;
using Bogus;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Guilds.Tributes.Entities;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class GuildGenerator
    {
        private const int GuildCount = 5;

        public List<Guild> Guilds { get; }
        public List<GuildMember> GuildMembers { get; }
        public List<GuildPinnedProject> PinnedProjects { get; }
        public List<Tribute> TributeEntities { get; }

        public GuildGenerator(List<Student> students, List<GithubProject> githubProjects)
        {
            var faker = new Faker<Guild>()
                .RuleFor(g => g.Id, f => f.IndexVariable++ + 1)
                .RuleFor(g => g.Title, f => f.Company.CompanyName())
                .RuleFor(g => g.Bio, f => f.Lorem.Paragraph())
                .RuleFor(g => g.LogoUrl, f => f.Image.PicsumUrl())
                .RuleFor(g => g.HiringPolicy, GuildHiringPolicy.Open)
                .RuleFor(g => g.GuildType, GuildType.Created);

            var pinnedFaker = new Faker<GuildPinnedProject>()
                .RuleFor(g => g.Id, f => f.IndexFaker + 1)
                .RuleFor(gp => gp.RepositoryOwner, f => f.Company.CompanyName())
                .RuleFor(gp => gp.RepositoryName, f => f.Company.CompanyName());


            Guilds = faker.Generate(GuildCount);
            GuildMembers = new List<GuildMember>();
            PinnedProjects = new List<GuildPinnedProject>();
            TributeEntities = new List<Tribute>();
            
            int usedCount = 0;
            foreach (Guild guild in Guilds)
            {
                var creator = new GuildMember(guild, students[usedCount], GuildMemberType.Creator);
                GuildMembers.Add(creator);
                usedCount++;
                
                List<GuildMember> members = students
                    .Skip(usedCount)
                    .Take(10)
                    .Select(s => new GuildMember(guild, s, GuildMemberType.Member))
                    .ToList();
                GuildMembers.AddRange(members);
                usedCount += 10;

                PinnedProjects.AddRange(pinnedFaker
                    .RuleFor(p => p.GuildId, guild.Id)
                    .Generate(5));

                foreach (var member in members.Where(m => m.MemberType == GuildMemberType.Member))
                {
                    var student = students.First(s => s.Id == member.MemberId);
                    var githubProjectEntity = githubProjects.First(p => p.Owner == student.GithubUsername);
                    var tributeEntity = Tribute.Create(guild, student, githubProjectEntity, new List<Tribute>());
                    TributeEntities.Add(tributeEntity);
                }
            }
        }
    }
}
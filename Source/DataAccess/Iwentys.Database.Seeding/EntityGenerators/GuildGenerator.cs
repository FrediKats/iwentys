using System.Collections.Generic;
using System.Linq;
using Bogus;
using Iwentys.Database.Seeding.FakerEntities.Guilds;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Guilds.Tributes.Entities;
using Iwentys.Features.Study.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class GuildGenerator : IEntityGenerator
    {
        private const int GuildCount = 5;
        private const int GuildMemberCount = 10;
        private const int GuildPinnedProjectCount = 5;

        public GuildGenerator(List<Student> students, List<GithubProject> githubProjects)
        {
            Faker<Guild> guildFaker = GuildFaker.Instance.CreateGuildFaker();

            Guilds = guildFaker.Generate(GuildCount);
            GuildMembers = new List<GuildMember>();
            PinnedProjects = new List<GuildPinnedProject>();
            TributeEntities = new List<Tribute>();

            var usedCount = 0;
            foreach (Guild guild in Guilds)
            {
                var creator = new GuildMember(guild, students[usedCount], GuildMemberType.Creator);
                GuildMembers.Add(creator);
                usedCount++;

                List<GuildMember> members = students
                    .Skip(usedCount)
                    .Take(GuildMemberCount)
                    .Select(s => new GuildMember(guild, s, GuildMemberType.Member))
                    .ToList();
                GuildMembers.AddRange(members);
                usedCount += GuildMemberCount;

                for (var i = 0; i < GuildPinnedProjectCount; i++) PinnedProjects.Add(GuildFaker.Instance.CreateGuildPinnedProject(guild.Id));

                foreach (GuildMember member in members.Where(m => m.MemberType == GuildMemberType.Member))
                {
                    Student student = students.First(s => s.Id == member.MemberId);
                    GithubProject githubProjectEntity = githubProjects.First(p => p.Owner == student.GithubUsername);
                    var tributeEntity = Tribute.Create(guild, student, githubProjectEntity, new List<Tribute>());
                    TributeEntities.Add(tributeEntity);
                }
            }
        }

        public List<Guild> Guilds { get; }
        public List<GuildMember> GuildMembers { get; }
        public List<GuildPinnedProject> PinnedProjects { get; }
        public List<Tribute> TributeEntities { get; }

        public void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Guild>().HasData(Guilds);
            modelBuilder.Entity<GuildMember>().HasData(GuildMembers);
            modelBuilder.Entity<GuildPinnedProject>().HasData(PinnedProjects);
            modelBuilder.Entity<Tribute>().HasData(TributeEntities);
        }
    }
}
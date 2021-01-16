using System.Collections.Generic;
using System.Linq;
using Iwentys.Database.Seeding.FakerEntities;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Newsfeeds.Entities;
using Iwentys.Features.Study.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class NewsfeedGenerator : IEntityGenerator
    {
        public List<Newsfeed> Newsfeeds { get; set; }
        public List<SubjectNewsfeed> SubjectNewsfeeds { get; set; }
        public List<GuildNewsfeed> GuildNewsfeeds { get; set; }

        public NewsfeedGenerator(List<Student> students, List<Guild> guilds, List<Subject> subjects)
        {
            var faker = NewsfeedFaker.Instance.CreateNewsfeedFaker(students.First().Id);

            Newsfeeds = new List<Newsfeed>();

            SubjectNewsfeeds = new List<SubjectNewsfeed>();
            foreach (Subject subject in subjects)
            {
                for (int i = 0; i < 3; i++)
                {
                    var newsfeedEntity = faker.Generate();
                    Newsfeeds.Add(newsfeedEntity);
                    SubjectNewsfeeds.Add(new SubjectNewsfeed { SubjectId = subject.Id, NewsfeedId = newsfeedEntity.Id });
                }
            }

            GuildNewsfeeds = new List<GuildNewsfeed>();
            foreach (Guild guild in guilds)
            {
                var newsfeedEntity = faker.Generate();
                Newsfeeds.Add(newsfeedEntity);
                GuildNewsfeeds.Add(new GuildNewsfeed { GuildId = guild.Id, NewsfeedId = newsfeedEntity.Id });
            }
        }

        public void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Newsfeed>().HasData(Newsfeeds);
            modelBuilder.Entity<SubjectNewsfeed>().HasData(SubjectNewsfeeds);
            modelBuilder.Entity<GuildNewsfeed>().HasData(GuildNewsfeeds);
        }
    }
}
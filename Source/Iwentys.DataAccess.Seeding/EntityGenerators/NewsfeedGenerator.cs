using System.Collections.Generic;
using System.Linq;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Newsfeeds;
using Iwentys.Domain.Study;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.DataAccess.Seeding
{
    public class NewsfeedGenerator : IEntityGenerator
    {
        public NewsfeedGenerator(List<Student> students, List<Guild> guilds, List<Subject> subjects)
        {
            Newsfeeds = new List<Newsfeed>();

            SubjectNewsfeeds = new List<SubjectNewsfeed>();
            foreach (Subject subject in subjects)
            {
                for (var i = 0; i < 3; i++)
                {
                    Newsfeed newsfeedEntity = NewsfeedFaker.Instance.CreateNewsfeed(students.First().Id);
                    Newsfeeds.Add(newsfeedEntity);
                    SubjectNewsfeeds.Add(new SubjectNewsfeed {SubjectId = subject.Id, NewsfeedId = newsfeedEntity.Id});
                }
            }

            GuildNewsfeeds = new List<GuildNewsfeed>();
            foreach (Guild guild in guilds)
            {
                Newsfeed newsfeedEntity = NewsfeedFaker.Instance.CreateNewsfeed(students.First().Id);
                Newsfeeds.Add(newsfeedEntity);
                GuildNewsfeeds.Add(new GuildNewsfeed {GuildId = guild.Id, NewsfeedId = newsfeedEntity.Id});
            }
        }

        public List<Newsfeed> Newsfeeds { get; set; }
        public List<SubjectNewsfeed> SubjectNewsfeeds { get; set; }
        public List<GuildNewsfeed> GuildNewsfeeds { get; set; }

        public void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Newsfeed>().HasData(Newsfeeds);
            modelBuilder.Entity<SubjectNewsfeed>().HasData(SubjectNewsfeeds);
            modelBuilder.Entity<GuildNewsfeed>().HasData(GuildNewsfeeds);
        }
    }
}
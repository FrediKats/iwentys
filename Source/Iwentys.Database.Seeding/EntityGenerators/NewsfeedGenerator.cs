using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Newsfeeds.Entities;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class NewsfeedGenerator
    {
        public List<Newsfeed> Newsfeeds { get; set; }
        public List<SubjectNewsfeed> SubjectNewsfeeds { get; set; }
        public List<GuildNewsfeed> GuildNewsfeeds { get; set; }

        public NewsfeedGenerator(List<Student> students, List<Guild> guilds, List<Subject> subjects)
        {
            var faker = new Faker();
            faker.IndexVariable++;

            Newsfeeds = new List<Newsfeed>();

            SubjectNewsfeeds = new List<SubjectNewsfeed>();
            foreach (Subject subject in subjects)
            {
                for (int i = 0; i < 3; i++)
                {
                    var newsfeedEntity = new Newsfeed
                    {
                        Id = faker.IndexVariable++,
                        AuthorId = students.First().Id,
                        Content = faker.Lorem.Paragraph(),
                        CreationTimeUtc = DateTime.UtcNow,
                        Title = faker.Lorem.Slug(),
                    };

                    Newsfeeds.Add(newsfeedEntity);
                    SubjectNewsfeeds.Add(new SubjectNewsfeed { SubjectId = subject.Id, NewsfeedId = newsfeedEntity.Id });
                }
            }

            GuildNewsfeeds = new List<GuildNewsfeed>();
            foreach (Guild guild in guilds)
            {
                var newsfeedEntity = new Newsfeed
                {
                    Id = faker.IndexVariable++,
                    AuthorId = students.First().Id,
                    Content = faker.Lorem.Paragraph(),
                    CreationTimeUtc = DateTime.UtcNow,
                    Title = faker.Lorem.Slug(),
                };

                Newsfeeds.Add(newsfeedEntity);
                GuildNewsfeeds.Add(new GuildNewsfeed { GuildId = guild.Id, NewsfeedId = newsfeedEntity.Id });
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Entities.Newsfeeds;
using Iwentys.Models.Entities.Study;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class NewsfeedGenerator
    {
        public List<NewsfeedEntity> Newsfeeds { get; set; }
        public List<SubjectNewsfeedEntity> SubjectNewsfeeds { get; set; }

        public NewsfeedGenerator(List<StudentEntity> students, List<GuildEntity> guilds, List<SubjectEntity> subjects)
        {
            var faker = new Faker();
            faker.IndexVariable++;

            Newsfeeds = new List<NewsfeedEntity>();
            SubjectNewsfeeds = new List<SubjectNewsfeedEntity>();

            foreach (SubjectEntity subject in subjects)
            {
                var newsfeedEntity = new NewsfeedEntity
                {
                    Id = faker.IndexVariable++,
                    AuthorId = students.First().Id,
                    Content = faker.Lorem.Paragraph(),
                    CreationTimeUtc = DateTime.UtcNow,
                    Title = faker.Lorem.Slug(),
                };

                Newsfeeds.Add(newsfeedEntity);
                SubjectNewsfeeds.Add(new SubjectNewsfeedEntity {SubjectId = subject.Id, NewsfeedId = newsfeedEntity.Id});
            }
        }
    }
}
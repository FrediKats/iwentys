using System;
using Bogus;
using Iwentys.Features.Newsfeeds.Entities;

namespace Iwentys.Database.Seeding.FakerEntities
{
    public class NewsfeedFaker : Faker<Newsfeed>
    {
        public NewsfeedFaker(int authorId)
        {
            this
                .RuleFor(n => n.Id, f => f.IndexFaker + 1)
                .RuleFor(n => n.AuthorId, authorId)
                .RuleFor(n => n.Title, f => f.Lorem.Slug())
                .RuleFor(n => n.Content, f => f.Lorem.Paragraph())
                .RuleFor(n => n.CreationTimeUtc, DateTime.UtcNow);
        }
    }
}
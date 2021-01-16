using System;
using Bogus;
using Iwentys.Features.Newsfeeds.Entities;
using Iwentys.Features.Newsfeeds.Models;

namespace Iwentys.Database.Seeding.FakerEntities
{
    public class NewsfeedFaker
    {
        public static readonly NewsfeedFaker Instance = new NewsfeedFaker();

        private readonly Faker _faker = new Faker();

        public Faker<Newsfeed> CreateNewsfeedFaker(int authorId)
        {
            return new Faker<Newsfeed>()
                .RuleFor(n => n.Id, f => f.IndexFaker + 1)
                .RuleFor(n => n.AuthorId, authorId)
                .RuleFor(n => n.Title, f => f.Lorem.Slug())
                .RuleFor(n => n.Content, f => f.Lorem.Paragraph())
                .RuleFor(n => n.CreationTimeUtc, DateTime.UtcNow);
        }

        public NewsfeedCreateViewModel GenerateNewsfeedCreateViewModel()
        {
            return new NewsfeedCreateViewModel
            {
                Title = _faker.Lorem.Word(),
                Content = _faker.Lorem.Sentence()
            };
        }
    }
}
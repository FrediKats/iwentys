﻿using System;
using Bogus;
using Iwentys.Domain.Newsfeeds;

namespace Iwentys.DataAccess.Seeding
{
    public class NewsfeedFaker
    {
        public static readonly NewsfeedFaker Instance = new NewsfeedFaker();

        private readonly Faker _faker = new Faker();

        public Newsfeed CreateNewsfeed(int authorId)
        {
            return CreateNewsfeedFaker(authorId).Generate();
        }

        public Faker<Newsfeed> CreateNewsfeedFaker(int authorId)
        {
            return new Faker<Newsfeed>()
                .RuleFor(n => n.Id, _ => _faker.IndexVariable++ + 1)
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
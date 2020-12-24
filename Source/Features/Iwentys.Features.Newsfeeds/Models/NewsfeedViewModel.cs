using System;
using System.Linq.Expressions;
using Iwentys.Features.Newsfeeds.Entities;
using Iwentys.Features.Students.Models;

namespace Iwentys.Features.Newsfeeds.Models
{
    public class NewsfeedViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreationTimeUtc { get; set; }
        public string SourceLink { get; set; }

        public StudentInfoDto Author { get; set; }

        public static Expression<Func<SubjectNewsfeedEntity, NewsfeedViewModel>> FromSubjectEntity =>
            entity =>
                new NewsfeedViewModel
                {
                    Id = entity.Newsfeed.Id,
                    Title = entity.Newsfeed.Title,
                    Content = entity.Newsfeed.Content,
                    CreationTimeUtc = entity.Newsfeed.CreationTimeUtc,
                    SourceLink = entity.Newsfeed.SourceLink,
                    Author = new StudentInfoDto(entity.Newsfeed.Author)
                };

        public static Expression<Func<GuildNewsfeedEntity, NewsfeedViewModel>> FromGuildEntity =>
            entity =>
                new NewsfeedViewModel
                {
                    Id = entity.Newsfeed.Id,
                    Title = entity.Newsfeed.Title,
                    Content = entity.Newsfeed.Content,
                    CreationTimeUtc = entity.Newsfeed.CreationTimeUtc,
                    SourceLink = entity.Newsfeed.SourceLink,
                    Author = new StudentInfoDto(entity.Newsfeed.Author)
                };
    }
}
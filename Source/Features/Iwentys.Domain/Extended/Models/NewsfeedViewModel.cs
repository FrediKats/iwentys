using System;
using System.Linq.Expressions;
using Iwentys.Domain.AccountManagement;

namespace Iwentys.Domain.Extended.Models
{
    public class NewsfeedViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreationTimeUtc { get; set; }
        public string SourceLink { get; set; }

        public IwentysUserInfoDto Author { get; set; }

        public static Expression<Func<SubjectNewsfeed, NewsfeedViewModel>> FromSubjectEntity =>
            entity =>
                new NewsfeedViewModel
                {
                    Id = entity.Newsfeed.Id,
                    Title = entity.Newsfeed.Title,
                    Content = entity.Newsfeed.Content,
                    CreationTimeUtc = entity.Newsfeed.CreationTimeUtc,
                    SourceLink = entity.Newsfeed.SourceLink,
                    Author = new IwentysUserInfoDto(entity.Newsfeed.Author)
                };

        public static Expression<Func<GuildNewsfeed, NewsfeedViewModel>> FromGuildEntity =>
            entity =>
                new NewsfeedViewModel
                {
                    Id = entity.Newsfeed.Id,
                    Title = entity.Newsfeed.Title,
                    Content = entity.Newsfeed.Content,
                    CreationTimeUtc = entity.Newsfeed.CreationTimeUtc,
                    SourceLink = entity.Newsfeed.SourceLink,
                    Author = new IwentysUserInfoDto(entity.Newsfeed.Author)
                };

        public static Expression<Func<Newsfeed, NewsfeedViewModel>> FromEntity =>
            entity =>
                new NewsfeedViewModel
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    Content = entity.Content,
                    CreationTimeUtc = entity.CreationTimeUtc,
                    SourceLink = entity.SourceLink,
                    Author = new IwentysUserInfoDto(entity.Author)
                };
    }
}
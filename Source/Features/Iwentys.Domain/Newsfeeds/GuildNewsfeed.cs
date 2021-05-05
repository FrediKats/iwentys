using System;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Newsfeeds.Dto;

namespace Iwentys.Domain.Newsfeeds
{
    public class GuildNewsfeed
    {
        public int GuildId { get; init; }
        public virtual Guild Guild { get; init; }

        public int NewsfeedId { get; init; }
        public virtual Newsfeed Newsfeed { get; init; }

        public static GuildNewsfeed Create(NewsfeedCreateViewModel createViewModel, GuildMentor author, Guild guild)
        {
            var newsfeed = new Newsfeed
            {
                Title = createViewModel.Title,
                Content = createViewModel.Content,
                CreationTimeUtc = DateTime.UtcNow,
                AuthorId = author.User.Id
            };

            var guildNewsfeed = new GuildNewsfeed
            {
                Newsfeed = newsfeed,
                GuildId = guild.Id
            };

            return guildNewsfeed;
        }
    }
}
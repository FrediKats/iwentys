using System;
using Iwentys.Features.Guilds.Domain;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Newsfeeds.Models;

namespace Iwentys.Features.Newsfeeds.Entities
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
                AuthorId = author.Student.Id
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
using Iwentys.Features.Guilds.Entities;

namespace Iwentys.Features.Newsfeeds.Entities
{
    public class GuildNewsfeed
    {
        public int GuildId { get; init; }
        public virtual Guild Guild { get; init; }

        public int NewsfeedId { get; init; }
        public virtual Newsfeed Newsfeed { get; init; }
    }
}
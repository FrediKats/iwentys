using Iwentys.Features.Guilds.Entities;

namespace Iwentys.Features.Newsfeeds.Entities
{
    public class GuildNewsfeed
    {
        public int GuildId { get; set; }
        public virtual Guild Guild { get; set; }

        public int NewsfeedId { get; set; }
        public virtual Newsfeed Newsfeed { get; set; }
    }
}
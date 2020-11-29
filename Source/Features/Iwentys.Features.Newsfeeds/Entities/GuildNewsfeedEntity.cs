using Iwentys.Models.Entities.Guilds;

namespace Iwentys.Features.Newsfeeds.Entities
{
    public class GuildNewsfeedEntity
    {
        public int GuildId { get; set; }
        public GuildEntity Guild { get; set; }

        public int NewsfeedId { get; set; }
        public NewsfeedEntity Newsfeed { get; set; }
    }
}
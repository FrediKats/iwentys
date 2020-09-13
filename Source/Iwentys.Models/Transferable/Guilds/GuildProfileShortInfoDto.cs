using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Types.Guilds;

namespace Iwentys.Models.Transferable.Guilds
{
    public class GuildProfileShortInfoDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Bio { get; set; }
        public string LogoUrl { get; set; }
        public string TestTaskLink { get; set; }

        public GuildHiringPolicy HiringPolicy { get; set; }

        public GuildProfileShortInfoDto()
        {
        }

        public GuildProfileShortInfoDto(GuildEntity guild) : this()
        {
            Id = guild.Id;
            Title = guild.Title;
            Bio = guild.Bio;
            LogoUrl = guild.LogoUrl;
            TestTaskLink = guild.TestTaskLink;
            HiringPolicy = guild.HiringPolicy;
        }
    }
}
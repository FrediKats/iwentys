using Iwentys.Models.Types.Guilds;

namespace Iwentys.Models.Transferable.Guilds
{
    public class GuildProfileShortInfoDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Bio { get; set; }
        public string LogoUrl { get; set; }

        public GuildHiringPolicy HiringPolicy { get; set; }
    }
}
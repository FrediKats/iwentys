using Iwentys.Models.Types;

namespace Iwentys.Database.Transferable.Guilds
{
    public class GuildCreateArgumentDto
    {
        public string Title { get; set; }
        public string Bio { get; set; }
        public string LogoUrl { get; set; }
        public GuildHiringPolicy HiringPolicy { get; set; }
    }
}
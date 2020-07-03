using Iwentys.Models.Types.Guilds;

namespace Iwentys.Models.Transferable.Guilds
{
    public class GuildUpdateArgumentDto
    {
        public int Id { get; set; }

        public string Bio { get; set; }
        public string LogoUrl { get; set; }
        public GuildHiringPolicy? HiringPolicy { get; set; }
    }
}
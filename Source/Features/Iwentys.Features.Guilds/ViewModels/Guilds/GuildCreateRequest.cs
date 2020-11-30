using Iwentys.Features.Guilds.Enums;

namespace Iwentys.Features.Guilds.ViewModels.Guilds
{
    public class GuildCreateRequest
    {
        public string Title { get; set; }
        public string Bio { get; set; }
        public string LogoUrl { get; set; }
        public GuildHiringPolicy HiringPolicy { get; set; }
    }
}
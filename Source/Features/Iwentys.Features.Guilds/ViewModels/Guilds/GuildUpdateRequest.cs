using Iwentys.Features.Guilds.Enums;

namespace Iwentys.Features.Guilds.ViewModels.Guilds
{
    public class GuildUpdateRequest
    {
        public int Id { get; set; }

        public string Bio { get; set; }
        public string LogoUrl { get; set; }
        public string TestTaskLink { get; set; }

        public GuildHiringPolicy? HiringPolicy { get; set; }
    }
}
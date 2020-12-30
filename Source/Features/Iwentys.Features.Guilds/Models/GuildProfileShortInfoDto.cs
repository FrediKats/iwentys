using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;

namespace Iwentys.Features.Guilds.Models
{
    public class GuildProfileShortInfoDto
    {
        public GuildProfileShortInfoDto()
        {
        }

        public GuildProfileShortInfoDto(Guild guild) : this()
        {
            Id = guild.Id;
            Title = guild.Title;
            Bio = guild.Bio;
            LogoUrl = guild.LogoUrl;
            TestTaskLink = guild.TestTaskLink;
            HiringPolicy = guild.HiringPolicy;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Bio { get; set; }
        public string LogoUrl { get; set; }
        public string TestTaskLink { get; set; }

        public GuildHiringPolicy HiringPolicy { get; set; }
    }
}
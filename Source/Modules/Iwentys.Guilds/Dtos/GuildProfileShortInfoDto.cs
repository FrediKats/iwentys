using Iwentys.Domain.Guilds;

namespace Iwentys.Modules.Guilds.Dtos
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
            ImmageUrl = guild.ImageUrl;
            TestTaskLink = guild.TestTaskLink;
            HiringPolicy = guild.HiringPolicy;
            GuildType = guild.GuildType;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Bio { get; set; }
        public string ImmageUrl { get; set; }
        public string TestTaskLink { get; set; }

        public GuildHiringPolicy HiringPolicy { get; set; }
        public GuildType GuildType { get; set; }
    }
}
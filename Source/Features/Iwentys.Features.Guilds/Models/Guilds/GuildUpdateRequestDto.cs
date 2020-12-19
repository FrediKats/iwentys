using Iwentys.Features.Guilds.Enums;

namespace Iwentys.Features.Guilds.Models.Guilds
{
    public record GuildUpdateRequestDto
    {
        public static GuildUpdateRequestDto ForPolicyUpdate(int guildId, GuildHiringPolicy hiringPolicy)
        {
            return new GuildUpdateRequestDto(guildId, null, null, null, hiringPolicy);
        }

        public GuildUpdateRequestDto(int id, string bio, string logoUrl, string testTaskLink, GuildHiringPolicy? hiringPolicy)
        {
            Id = id;
            Bio = bio;
            LogoUrl = logoUrl;
            TestTaskLink = testTaskLink;
            HiringPolicy = hiringPolicy;
        }

        public GuildUpdateRequestDto()
        {
        }

        public int Id { get; init; }
        public string Bio { get; init; }
        public string LogoUrl { get; init; }
        public string TestTaskLink { get; init; }
        public GuildHiringPolicy? HiringPolicy { get; init; }
    }
}
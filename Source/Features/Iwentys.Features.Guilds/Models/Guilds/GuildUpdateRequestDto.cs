using Iwentys.Features.Guilds.Enums;

namespace Iwentys.Features.Guilds.Models.Guilds
{
    public record GuildUpdateRequestDto(
        int Id,
        string Bio,
        string LogoUrl,
        string TestTaskLink,
        GuildHiringPolicy? HiringPolicy)
    {
        public static GuildUpdateRequestDto ForPolicyUpdate(int guildId, GuildHiringPolicy hiringPolicy)
        {
            return new GuildUpdateRequestDto(guildId, null, null, null, hiringPolicy);
        }
    }
}
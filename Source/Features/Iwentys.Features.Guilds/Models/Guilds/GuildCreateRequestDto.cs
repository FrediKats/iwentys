using Iwentys.Features.Guilds.Enums;

namespace Iwentys.Features.Guilds.Models.Guilds
{
    public record GuildCreateRequestDto(
        string Title,
        string Bio,
        string LogoUrl,
        GuildHiringPolicy HiringPolicy)
    {
    }
}
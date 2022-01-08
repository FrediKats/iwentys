namespace Iwentys.Domain.Guilds;

public record GuildCreateRequestDto
{
    public GuildCreateRequestDto(string title, string bio, string logoUrl, GuildHiringPolicy hiringPolicy)
    {
        Title = title;
        Bio = bio;
        LogoUrl = logoUrl;
        HiringPolicy = hiringPolicy;
    }

    public GuildCreateRequestDto()
    {
    }

    public string Title { get; init; }
    public string Bio { get; init; }
    public string LogoUrl { get; init; }
    public GuildHiringPolicy HiringPolicy { get; init; }
}
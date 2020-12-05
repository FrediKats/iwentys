namespace Iwentys.Features.GithubIntegration.Models
{
    public record GithubUserInfoDto(
        int Id,
        string Name,
        string AvatarUrl,
        string Bio,
        string Company)
    {
    }
}
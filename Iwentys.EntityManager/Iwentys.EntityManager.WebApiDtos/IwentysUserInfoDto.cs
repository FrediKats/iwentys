namespace Iwentys.EntityManager.WebApiDtos;

public class IwentysUserInfoDto : UniversitySystemUserInfoDto
{
    public bool IsAdmin { get; set; }
    public string GithubUsername { get; set; }
    public DateTime CreationTime { get; init; }
    public DateTime LastOnlineTime { get; set; }
    public string AvatarUrl { get; set; }
}
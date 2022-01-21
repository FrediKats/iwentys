using Iwentys.EntityManager.Domain.Accounts;

namespace Iwentys.EntityManager.WebApi;

public class IwentysUserInfoDto : UniversitySystemUserInfoDto
{
    public IwentysUserInfoDto(IwentysUser user) : base(user)
    {
        IsAdmin = user.IsAdmin;
        GithubUsername = user.GithubUsername;
        CreationTime = user.CreationTime;
        LastOnlineTime = user.LastOnlineTime;
        AvatarUrl = user.AvatarUrl;
    }

    public IwentysUserInfoDto()
    {
    }

    public bool IsAdmin { get; set; }
    public string GithubUsername { get; set; }
    public DateTime CreationTime { get; init; }
    public DateTime LastOnlineTime { get; set; }
    public string AvatarUrl { get; set; }
}
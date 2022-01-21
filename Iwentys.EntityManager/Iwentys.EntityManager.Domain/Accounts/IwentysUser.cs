namespace Iwentys.EntityManager.Domain.Accounts;

public class IwentysUser : UniversitySystemUser
{
    public bool IsAdmin { get; set; }
    public string GithubUsername { get; set; }
    public DateTime CreationTime { get; init; }
    public DateTime LastOnlineTime { get; set; }
    public string AvatarUrl { get; set; }

    public void UpdateGithubUsername(string githubUsername)
    {
        GithubUsername = githubUsername;
    }
}
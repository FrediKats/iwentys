using System;

namespace Iwentys.Domain.AccountManagement
{
    public class IwentysUser : UniversitySystemUser
    {
        public bool IsAdmin { get; set; }
        public string GithubUsername { get; set; }
        public DateTime CreationTime { get; init; }
        public DateTime LastOnlineTime { get; set; }
        public int BarsPoints { get; set; }
        public string AvatarUrl { get; set; }

        public static IwentysUser Create(IwentysUserCreateArguments createArguments)
        {
            return new IwentysUser
            {
                IsAdmin = createArguments.IsAdmin,
                GithubUsername = createArguments.GithubUsername,
                BarsPoints = createArguments.BarsPoints,
                AvatarUrl = createArguments.AvatarUrl,
                Id = createArguments.Id ?? 0,
                FirstName = createArguments.FirstName,
                MiddleName = createArguments.MiddleName,
                SecondName = createArguments.SecondName
            };
        }

        public void UpdateGithubUsername(string githubUsername)
        {
            GithubUsername = githubUsername;
        }
    }
}
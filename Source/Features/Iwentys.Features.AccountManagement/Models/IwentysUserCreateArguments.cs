namespace Iwentys.Features.AccountManagement.Models
{
    public class IwentysUserCreateArguments : UniversitySystemUserCreateArguments
    {
        public bool IsAdmin { get; set; }
        public string GithubUsername { get; set; }
        public int BarsPoints { get; set; }
        public string AvatarUrl { get; set; }

        public IwentysUserCreateArguments(int? id, string firstName, string middleName, string secondName, bool isAdmin, string githubUsername, int barsPoints, string avatarUrl)
            : base(id, firstName, middleName, secondName)
        {
            IsAdmin = isAdmin;
            GithubUsername = githubUsername;
            BarsPoints = barsPoints;
            AvatarUrl = avatarUrl;
        }
    }
}
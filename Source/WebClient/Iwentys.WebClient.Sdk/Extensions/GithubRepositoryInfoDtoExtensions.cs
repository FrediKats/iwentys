using Iwentys.Sdk;

namespace Iwentys.WebClient.Sdk
{
    public static class GithubRepositoryInfoDtoExtensions
    {
        public static string GithubLikeTitle(this GithubRepositoryInfoDto githubRepositoryInfo)
        {
            return $"{githubRepositoryInfo.Owner}/{githubRepositoryInfo.Name}";
        }
    }
}
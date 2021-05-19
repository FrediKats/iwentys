using Iwentys.Sdk;

namespace Iwentys.Endpoints.Sdk.Extensions
{
    public static class GithubRepositoryInfoDtoExtensions
    {
        public static string GithubLikeTitle(this GithubRepositoryInfoDto githubRepositoryInfo)
        {
            return $"{githubRepositoryInfo.Owner}/{githubRepositoryInfo.Name}";
        }
    }
}
using System.Collections.Generic;
using Iwentys.Models.Types;
using Octokit;

namespace Iwentys.Core.GithubIntegration
{
    public interface IGithubApiAccessor
    {
        GithubRepository GetRepository(string username, string repositoryName);
        IReadOnlyList<GithubRepository> GetUserRepositories(string username);

        GithubUser GetGithubUser(string githubUsername);
    }
}
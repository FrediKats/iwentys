using Iwentys.DataAccess.Seeding;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.GithubIntegration;

namespace Iwentys.Tests.TestCaseContexts
{
    public class GithubTestCaseContext
    {
        public GithubProject WithStudentProject(IwentysUser user, GithubUser githubUser)
        {
            GithubRepositoryInfoDto repositoryInfo = GithubRepositoryFaker.Instance.Generate(user.GithubUsername);

            var githubProject = new GithubProject(githubUser, repositoryInfo);
            //FYI: force EF to generate unique id
            githubProject.Id = 0;

            return githubProject;
        }
    }
}
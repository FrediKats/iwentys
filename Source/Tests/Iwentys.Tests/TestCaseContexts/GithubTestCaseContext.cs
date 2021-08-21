using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.GithubIntegration.Models;
using Iwentys.Infrastructure.DataAccess.Seeding.FakerEntities;

namespace Iwentys.Tests.TestCaseContexts
{
    public class GithubTestCaseContext
    {
        private readonly TestCaseContext _context;

        public GithubTestCaseContext(TestCaseContext context)
        {
            _context = context;
        }

        public GithubProject WithStudentProject(IwentysUser user, GithubUser githubUser)
        {
            GithubRepositoryInfoDto repositoryInfo = GithubRepositoryFaker.Instance.Generate(user.GithubUsername);

            var githubProject = new GithubProject(githubUser, repositoryInfo);
            //FYI: force EF to generate unique id
            githubProject.Id = 0;

            return githubProject;
        }

        public GithubUser WithGithubAccount(IwentysUser user)
        {
            var newGithubUser = new GithubUser
            {
                IwentysUserId = user.Id,
                Username = user.GithubUsername
            };
            _context._context.GithubUsersData.Add(newGithubUser);
            _context._context.SaveChanges();

            return newGithubUser;
        }
    }
}
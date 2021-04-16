using Iwentys.Common.Databases;
using Iwentys.Database.Seeding.FakerEntities;
using Iwentys.Domain;
using Iwentys.Domain.Models;

namespace Iwentys.Tests.TestCaseContexts
{
    public class GithubTestCaseContext
    {
        private readonly TestCaseContext _context;

        public GithubTestCaseContext(TestCaseContext context)
        {
            _context = context;
        }

        public GithubProject WithStudentProject(AuthorizedUser userInfo)
        {
            IwentysUser student = _context.UnitOfWork.GetRepository<IwentysUser>().GetById(userInfo.Id).Result;
            GithubUser githubUser = _context.GithubIntegrationService.User.Get(userInfo.Id).Result;
            GithubRepositoryInfoDto repositoryInfo = GithubRepositoryFaker.Instance.Generate(student.GithubUsername);

            var githubProject = new GithubProject(githubUser, repositoryInfo);
            //FYI: force EF to generate unique id
            githubProject.Id = 0;

            _context.UnitOfWork.GetRepository<GithubProject>().InsertAsync(githubProject).Wait();
            _context.UnitOfWork.CommitAsync().Wait();

            return githubProject;
        }

        public GithubUser WithGithubAccount(AuthorizedUser user)
        {
            IwentysUser iwentysUser = _context.UnitOfWork.GetRepository<IwentysUser>().GetById(user.Id).Result;
            var newGithubUser = new GithubUser
            {
                IwentysUserId = iwentysUser.Id,
                Username = iwentysUser.GithubUsername
            };
            _context.UnitOfWork.GetRepository<GithubUser>().InsertAsync(newGithubUser).Wait();
            _context.UnitOfWork.CommitAsync().Wait();

            return newGithubUser;
        }
    }
}

using Iwentys.Common.Databases;
using Iwentys.Database.Seeding.FakerEntities;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Tests.TestCaseContexts
{
    public partial class TestCaseContext
    {
        public TestCaseContext WithStudentProject(AuthorizedUser userInfo, out GithubProject githubProject)
        {
            Student student = this.UnitOfWork.GetRepository<Student>().GetByIdAsync(userInfo.Id).Result;
            GithubRepositoryInfoDto repositoryInfo = GithubRepositoryFaker.Instance.Generate(student.GithubUsername);

            githubProject = new GithubProject(student, repositoryInfo);
            //FYI: force EF to generate unique id
            githubProject.Id = 0;

            UnitOfWork.GetRepository<GithubProject>().InsertAsync(githubProject).Wait();
            UnitOfWork.CommitAsync().Wait();

            return this;
        }
    }
}

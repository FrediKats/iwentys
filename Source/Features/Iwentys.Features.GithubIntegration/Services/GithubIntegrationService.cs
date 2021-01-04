using Iwentys.Common.Databases;

namespace Iwentys.Features.GithubIntegration.Services
{
    public class GithubIntegrationService
    {
        public readonly GithubUserApiAccessor User;
        public readonly GithubRepositoryApiAccessor Repository;

        public GithubIntegrationService(IGithubApiAccessor githubApiAccessor, IUnitOfWork unitOfWork)
        {
            User = new GithubUserApiAccessor(githubApiAccessor, unitOfWork);
            Repository = new GithubRepositoryApiAccessor(githubApiAccessor, unitOfWork, User);
        }
    }
}
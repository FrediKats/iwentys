using Iwentys.Common.Databases;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.GithubIntegration.Models;

namespace Iwentys.Infrastructure.Application.GithubIntegration
{
    public class GithubIntegrationService
    {
        private readonly IGithubApiAccessor _githubApiAccessor;
        public readonly GithubRepositoryApiAccessor Repository;
        public readonly GithubUserApiAccessor User;

        public GithubIntegrationService(IGithubApiAccessor githubApiAccessor, IUnitOfWork unitOfWork)
        {
            _githubApiAccessor = githubApiAccessor;
            User = new GithubUserApiAccessor(githubApiAccessor, unitOfWork);
            Repository = new GithubRepositoryApiAccessor(githubApiAccessor, unitOfWork, User);
        }

        public OrganizationInfoDto FindOrganizationInfo(string organizationName)
        {
            return _githubApiAccessor.FindOrganizationInfo(organizationName);
        }
    }
}
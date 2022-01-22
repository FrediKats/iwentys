using Iwentys.DataAccess;
using Iwentys.Domain.GithubIntegration;
using Iwentys.EntityManagerServiceIntegration;

namespace Iwentys.WebService.Application;

public class GithubIntegrationService
{
    private readonly IGithubApiAccessor _githubApiAccessor;
    public readonly GithubRepositoryApiAccessor Repository;
    public readonly GithubUserApiAccessor User;

    public GithubIntegrationService(IGithubApiAccessor githubApiAccessor, IwentysDbContext context, TypedIwentysEntityManagerApiClient entityManagerApiClient)
    {
        _githubApiAccessor = githubApiAccessor;
        User = new GithubUserApiAccessor(githubApiAccessor, context, entityManagerApiClient);
        Repository = new GithubRepositoryApiAccessor(githubApiAccessor, User, context);
    }

    public OrganizationInfoDto FindOrganizationInfo(string organizationName)
    {
        return _githubApiAccessor.FindOrganizationInfo(organizationName);
    }
}
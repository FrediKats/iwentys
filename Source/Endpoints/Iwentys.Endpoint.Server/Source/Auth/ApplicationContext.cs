using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Iwentys.Endpoint.Server.Source.Auth
{
    public class ApplicationContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options, IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {
            Database.EnsureCreated();
        }
    }
}
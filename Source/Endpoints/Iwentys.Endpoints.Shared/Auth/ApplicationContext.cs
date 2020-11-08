using IdentityServer4.EntityFramework.Options;
using Iwentys.Endpoint.Server.Models;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Iwentys.Endpoints.Shared.Auth
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
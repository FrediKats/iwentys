using Duende.IdentityServer.EntityFramework.Options;
using Iwentys.Domain.AccountManagement;
using Iwentys.EntityManagerServiceIntegration;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Iwentys.WebService.AuthComponents;

public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        this.SeedRoles(builder);
    }

    public void SeedUsers(UserManager<ApplicationUser> userManager, TypedIwentysEntityManagerApiClient entityManagerApiClient)
    {
        var iwentysUsers = entityManagerApiClient.IwentysUserProfiles.GetAsync().Result;
        foreach (IwentysUser iwentysUser in iwentysUsers)
        {
            ApplicationUser user = new ApplicationUser
            {
                Id = iwentysUser.Id.ToString(),
                UserName = iwentysUser.Id.ToString(),
                //UserName = $"{iwentysUser.FirstName} {iwentysUser.SecondName}",
            };

            IdentityResult identityResult = userManager.CreateAsync(user, iwentysUser.Id.ToString()).Result;
        }
    }

    private void SeedRoles(ModelBuilder builder)
    {
        builder.Entity<IdentityRole>().HasData(
            new IdentityRole() { Id = "fab4fac1-c546-41de-aebc-a14da6895711", Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" }
        );
    }
}
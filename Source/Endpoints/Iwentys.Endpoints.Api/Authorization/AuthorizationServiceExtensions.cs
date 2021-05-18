using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Endpoints.Api.Authorization
{
    public static class AuthorizationServiceExtensions
    {
        public static IServiceCollection ConfigureIdentityFramework(this IServiceCollection services)
        {
            //TODO: load from config
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Data Source=identity.db"));

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

            return services;
        }

        public static IApplicationBuilder ConfigureIdentityFramework(this IApplicationBuilder app)
        {
            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
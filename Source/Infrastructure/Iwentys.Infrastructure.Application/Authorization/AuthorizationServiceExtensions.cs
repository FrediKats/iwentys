﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Infrastructure.Application.Authorization
{
    public static class AuthorizationServiceExtensions
    {
        public static IServiceCollection ConfigureIdentityFramework(this IServiceCollection services)
        {
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

            //TODO: reconfig later
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 1;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 1;
            });

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
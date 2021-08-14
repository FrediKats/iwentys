using System.Text.Json.Serialization;
using Iwentys.Endpoints.Api.Authorization;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.Configuration;
using Iwentys.Infrastructure.DataAccess;
using Iwentys.Integrations.IsuIntegration.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Endpoints.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIwentysIdentity();
            services.EnableExceptional();
            services
                .AddControllersWithViews(options =>
            {
                options.Filters.Add(new ProducesAttribute("application/json"));
                options.Filters.Add(typeof(ModelStateFilter));
            })
                .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            services.AddSwaggerGen();
            services.AddRazorPages();

            services
                .AddIsuIntegrationOptions(Configuration)
                .AddIwentysOptions(Configuration)
                .AddIwentysLogging()
                .AddIwentysCorsHack()
                .AddIwentysDatabase()
                .AddIwentysSeeder()
                .AddUnitOfWork<IwentysDbContext>()
                .AddIwentysMediatorHandlers()
                .AddIwentysServices()
                .AddAutoMapperConfig();
        }

        public void Configure(IApplicationBuilder app, IwentysDbContext db, ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager)
        {
            app.UseExceptional();
            app.UseMigrationsEndPoint();
            app.UseWebAssemblyDebugging();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Unstable API v0.1");
                c.RoutePrefix = "swagger";
            });

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            //TODO: for test propose
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            app.ConfigureIdentityFramework();
            applicationDbContext.Database.EnsureDeleted();
            applicationDbContext.Database.EnsureCreated();
            applicationDbContext.SeedUsers(userManager, db);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}

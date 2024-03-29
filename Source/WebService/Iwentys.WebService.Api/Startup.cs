using System.Text.Json.Serialization;
using Iwentys.DataAccess;
using Iwentys.DataAccess.Seeding;
using Iwentys.Endpoints.Api;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.IsuIntegration.Configuration;
using Iwentys.WebService.Application;
using Iwentys.WebService.AuthComponents;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.WebService.Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddIwentysIdentity(Configuration);
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
            .AddIwentysMediatorHandlers()
            .AddIwentysEntityManagerIntegration(Configuration)
            .AddIwentysServices()
            .AddAutoMapperConfig()
            .AddIwentysModules();
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

        using (IServiceScope serviceScope = app.ApplicationServices.CreateScope())
        {
            var iwentysEntityManagerApiClient = serviceScope.ServiceProvider.GetRequiredService<TypedIwentysEntityManagerApiClient>();

            //TODO: for test propose
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            var databaseSynchronization = new EntityManagerDatabaseSynchronization(db, iwentysEntityManagerApiClient);
            databaseSynchronization.UpdateStudentGroup().Wait();

            app.ConfigureIdentityFramework();
            applicationDbContext.Database.EnsureDeleted();
            applicationDbContext.Database.EnsureCreated();
            applicationDbContext.SeedUsers(userManager, iwentysEntityManagerApiClient);
        }
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
            endpoints.MapControllers();
            endpoints.MapFallbackToFile("index.html");
        });
    }
}
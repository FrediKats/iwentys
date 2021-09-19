using Iwentys.AccountManagement.Server.Authorization;
using Iwentys.Infrastructure.DataAccess;
using Iwentys.Infrastructure.DataAccess.Seeding;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Iwentys.AccountManagement.Server
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
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddIwentysIdentity(Configuration);

            services.AddControllersWithViews();
            services.AddRazorPages();

            services
                .AddDbContext<IwentysDbContext>(o => o
                .UseLazyLoadingProxies()
                .UseInMemoryDatabase("Data Source=Iwentys.db"));
            
            services
                .AddIwentysSeeder();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IwentysDbContext db, ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

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

using System.Text.Json.Serialization;
using Iwentys.Endpoints.Api.Authorization;
using Iwentys.Endpoints.Api.Source;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.Configuration;
using Iwentys.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
            //TODO: load from config
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Data Source=identity.db"));
            services.ConfigureIdentityFramework();

            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddExceptional(settings =>
            {
                settings.Store.ApplicationName = "Samples.AspNetCore";
            });
            
            services.AddControllersWithViews().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            services.AddSwaggerGen();
            services.AddRazorPages();

            services
                .AddIwentysOptions(Configuration)
                .AddIwentysLogging()
                .AddIwentysCorsHack()
                .AddIwentysDatabase()
                .AddUnitOfWork<IwentysDbContext>()
                .AddIwentysMediatorHandlers()
                .AddIwentysServices()
                .AddAutoMapperConfig();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IwentysDbContext db, ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager)
        {
            app.UseExceptional();
            app.UseMigrationsEndPoint();
            app.UseWebAssemblyDebugging();
            //FYI: https://github.com/NickCraver/StackExchange.Exceptional/blob/main/samples/Samples.AspNetCore/Startup.cs
            //if (env.IsDevelopment())
            //{
            //    //app.UseDeveloperExceptionPage();
            //    app.UseMigrationsEndPoint();
            //    app.UseWebAssemblyDebugging();
            //}
            //else
            //{
            //    //app.UseExceptionHandler("/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}

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

            app.ConfigureIdentityFramework();
            applicationDbContext.Database.EnsureDeleted();
            applicationDbContext.Database.EnsureCreated();
            applicationDbContext.SeedUsers(userManager);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
            
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }
    }
}

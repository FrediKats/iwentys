using Iwentys.Database.Context;
using Iwentys.Endpoints.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Iwentys.Endpoints.WebUi
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
            services.AddIwentysLogging(Configuration);

            services.AddRazorPages();
            services.AddServerSideBlazor();

            services
                .AddApplicationOptions(Configuration)
                .AddIwentysDatabase(Configuration)
                //FYI: Token is required
                //.AddIwentysTokenFactory(Configuration)
                .AddIwentysServices(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IwentysDbContext db)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }
    }
}

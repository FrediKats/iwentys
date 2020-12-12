using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Iwentys.Database.Context;
using Iwentys.Endpoint.Server.Source.Data;
using Iwentys.Endpoint.Server.Source.Models;
using Iwentys.Endpoint.Server.Source.Tools;
using Microsoft.Extensions.Hosting;

namespace Iwentys.Endpoint.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //TODO: need refactor. I'm not sure about right order
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite("IdentDb"));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddExceptional(settings =>
            {
                settings.Store.ApplicationName = "Samples.AspNetCore";
            });
            
            //TODO: fix
            services.AddControllersWithViews();/*.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));*/
            services.AddSwaggerGen();
            services.AddRazorPages();
            
            services
                .AddIwentysLogging(Configuration)
                .AddIwentysCorsHack(Configuration)
                .AddApplicationOptions(Configuration)
                .AddIwentysDatabase()
                .AddIwentysTokenFactory(Configuration)
                .AddIwentysServices()
                .AddUnitOfWork<IwentysDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IwentysDbContext db)
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

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

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

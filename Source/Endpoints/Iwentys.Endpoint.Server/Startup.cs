using Iwentys.Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Iwentys.Endpoint.Server.Source.IdentityAuth;
using Iwentys.Endpoint.Server.Source.Tools;

namespace Iwentys.Endpoint.Server
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite("IdentDb"));

            services.AddLegacyIdentityAuth();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddExceptional(settings =>
            {
                settings.Store.ApplicationName = "Samples.AspNetCore";
            });
            
            //TODO: fix
            services.AddControllersWithViews();/*.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));*/
            services.AddSwaggerGen();
            services.AddRazorPages();

            services
                .AddIwentysOptions(Configuration)
                .AddIwentysLogging()
                .AddIwentysCorsHack()
                .AddIwentysDatabase()
                .AddUnitOfWork<IwentysDbContext>()
                .AddIwentysTokenFactory(Configuration)
                .AddIwentysServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IwentysDbContext db)
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

using System;
using System.Text.Json.Serialization;
using Iwentys.Core.Auth;
using Iwentys.Core.GithubIntegration;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Core.Services.Implementations;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Database.Repositories.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Iwentys.Api
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
            //TODO: Temp fix for CORS
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            // TODO: debug security key
            const string signingSecurityKey = "0d5b3235a8b403c3dab9c3f4f65c07fcalskd234n1k41230";
            var signingKey = new SigningSymmetricKey(signingSecurityKey);
            services.AddSingleton<IJwtSigningEncodingKey>(signingKey);

            services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            services.AddSwaggerGen();

            services.AddDbContext<IwentysDbContext>(o => o.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            //TODO: replace with GithubApiAccessor implementation
            services.AddScoped<IGithubApiAccessor, DummyGithubApiAccessor>();

            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IGuildRepository, GuildRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<ITournamentRepository, TournamentRepository>();
            services.AddScoped<IStudentProjectRepository, StudentProjectRepository>();
            services.AddScoped<ITributeRepository, TributeRepository>();
            services.AddScoped<IBarsPointTransactionLogRepository, BarsPointTransactionLogRepository>();
            services.AddScoped<IQuestRepository, QuestRepository>();
            services.AddScoped<ISubjectActivityRepository, SubjectActivityRepository>();
            services.AddScoped<ISubjectForGroupRepository, SubjectForGroupRepository>();

            services.AddScoped<DatabaseAccessor>();

            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IGuildService, GuildService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ITournamentService, TournamentService>();
            services.AddScoped<IBarsPointTransactionLogService, BarsPointTransactionLogService>();

//#if DEBUG
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
//#else
//            services.AddSpaStaticFiles(configuration =>
//            {

//                configuration.RootPath = "front/build/";
//            });
//#endif
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IwentysDbContext db)
        {
            //TODO: Temp fix for CORS
            app.UseCors("CorsPolicy");

            //FYI: We need to remove dev exception page after release
            //if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            app.UseDeveloperExceptionPage();

            db.Database.EnsureCreated();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = "swagger";
            });

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
#if DEBUG
            app.UseSpaStaticFiles();
#else
            app.UseSpaStaticFiles();
#endif

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

//#if DEBUG
            app.UseSpa(spa =>
            {

                spa.Options.SourcePath = "ClientApp";

                //TODO:
                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
//#else
//            app.UseSpa(spa =>
//            {

//                spa.Options.SourcePath = "front/";

//                //TODO:
//                if (env.IsDevelopment())
//                {
//                    spa.UseReactDevelopmentServer(npmScript: "start");
//                }
//            });
//#endif
        }
    }
}
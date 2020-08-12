using System.Text.Json.Serialization;
using Iwentys.Core;
using Iwentys.Core.Daemons;
using Iwentys.Core.Auth;
using Iwentys.Core.Gamification;
using Iwentys.Core.GithubIntegration;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Core.Services.Implementations;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Database.Repositories.Implementations;
using Iwentys.IsuIntegrator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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

            services.AddDbContext<IwentysDbContext>(o => o.UseSqlite("Data Source=Iwentys.db")
                .EnableSensitiveDataLogging(Configuration.GetValue<bool>("Logging:EnableSqlParameterLogging")));

            ApplicationOptions.GoogleServiceToken = Configuration["GoogleTable:Credentials"];

            //TODO: replace with GithubApiAccessor implementation
            services.AddScoped<IGithubApiAccessor, DummyGithubApiAccessor>();
            services.AddScoped<IIsuAccessor, DebugIsuAccessor>();

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
            services.AddScoped<AchievementProvider>();


            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IGuildService, GuildService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ITournamentService, TournamentService>();
            services.AddScoped<IBarsPointTransactionLogService, BarsPointTransactionLogService>();

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IwentysDbContext db,
            ISubjectActivityRepository subjectActivityRepository,
            ISubjectForGroupRepository subjectForGroupRepository,
            ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile(Configuration["LogFilePath"]);

            //TODO: Temp fix for CORS
            app.UseCors("CorsPolicy");

            //FYI: We need to remove dev exception page after release
            //if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            app.UseDeveloperExceptionPage();

            

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = "swagger";
            });
            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                //TODO:
                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            DaemonManager.Init(subjectActivityRepository, subjectForGroupRepository);
        }
    }
}
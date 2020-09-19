using System.Text.Json.Serialization;
using Iwentys.Core;
using Iwentys.Core.Auth;
using Iwentys.Core.Gamification;
using Iwentys.Core.GithubIntegration;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Core.Services.Implementations;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Database.Repositories.Implementations;
using Iwentys.IsuIntegrator;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

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
            ApplicationOptions.GoogleServiceToken = Configuration["GoogleTableCredentials"];
            ApplicationOptions.GithubToken = Configuration["GithubToken"];
            ApplicationOptions.TelegramToken = Configuration["TelegramToken"];
            ApplicationOptions.SigningSecurityKey = Configuration["jwt:SigningSecurityKey"];
            ApplicationOptions.JwtIssuer = Configuration["jwt:issuer"];

            //TODO: Temp fix for CORS
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            var signingKey = new SigningSymmetricKey(ApplicationOptions.SigningSecurityKey);
            services.AddSingleton<IJwtSigningEncodingKey>(signingKey);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = ApplicationOptions.JwtIssuer,
                        ValidAudience = ApplicationOptions.JwtIssuer,
                        IssuerSigningKey = signingKey.GetKey()
                    };
                });

            services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            services.AddSwaggerGen();

            services.AddDbContext<IwentysDbContext>(o => o.UseSqlite("Data Source=Iwentys.db")
                .EnableSensitiveDataLogging(Configuration.GetValue<bool>("Logging:EnableSqlParameterLogging")));

            if (ApplicationOptions.GithubToken is null)
                services.AddScoped<IGithubApiAccessor, DummyGithubApiAccessor>();
            else
                services.AddScoped<IGithubApiAccessor, GithubApiAccessor>();

            services.AddScoped<IIsuAccessor, DebugIsuAccessor>();

            services.AddScoped<IBarsPointTransactionLogRepository, BarsPointTransactionLogRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IGithubUserDataRepository, GithubUserDataRepository>();
            services.AddScoped<IGroupSubjectRepository, GroupGroupSubjectRepository>();
            services.AddScoped<IGuildMemberRepository, GuildMemberRepository>();
            services.AddScoped<IGuildRepository, GuildRepository>();
            services.AddScoped<IGuildTestTaskSolvingInfoRepository, GuildTestTaskSolvingInfoRepository>();
            services.AddScoped<IQuestRepository, QuestRepository>();
            services.AddScoped<IStudentProjectRepository, StudentProjectRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IStudyGroupRepository, StudyGroupRepository>();
            services.AddScoped<ISubjectActivityRepository, SubjectActivityRepository>();
            services.AddScoped<ITournamentRepository, TournamentRepository>();
            services.AddScoped<ITributeRepository, TributeRepository>();

            services.AddScoped<DatabaseAccessor>();
            services.AddScoped<AchievementProvider>();

            services.AddScoped<IBarsPointTransactionLogService, BarsPointTransactionLogService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IGithubUserDataService, GithubUserDataService>();
            services.AddScoped<IGuildMemberService, GuildMemberService>();
            services.AddScoped<IGuildService, GuildService>();
            services.AddScoped<IGuildTestTaskService, GuildTestTaskService>();
            services.AddScoped<IGuildTributeService, GuildTributeService>();
            services.AddScoped<IQuestService, QuestService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IStudyLeaderboardService, StudyLeaderboardService>();
            services.AddScoped<ITournamentService, TournamentService>();

            services.AddSpaStaticFiles(configuration => configuration.RootPath = "ClientApp/build");
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IwentysDbContext db)
        {
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());

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
        }
    }
}
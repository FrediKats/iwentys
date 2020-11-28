using Iwentys.Core.Services;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories;
using Iwentys.Database.Repositories.Achievements;
using Iwentys.Database.Repositories.Economy;
using Iwentys.Database.Repositories.GithubIntegration;
using Iwentys.Database.Repositories.Guilds;
using Iwentys.Endpoint.Server.Source.Auth;
using Iwentys.Features.Achievements;
using Iwentys.Features.Economy.Repositories;
using Iwentys.Features.GithubIntegration;
using Iwentys.Features.GithubIntegration.Repositories;
using Iwentys.Features.Guilds;
using Iwentys.Features.Guilds.Repositories;
using Iwentys.Features.Guilds.Services;
using Iwentys.Features.StudentFeature.Repositories;
using Iwentys.Features.StudentFeature.Services;
using Iwentys.Integrations.GithubIntegration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Iwentys.Endpoint.Server.Source
{
    public static class AspStartupExtensions
    {
        public static IServiceCollection ConfigIwentysOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddIwentysLogging(configuration)
                .AddIwentysCorsHack(configuration)
                .AddApplicationOptions(configuration)
                .AddIwentysDatabase()
                .AddIwentysTokenFactory(configuration)
                .AddIwentysServices();
            //TODO: meh (
            //.AddIwentysFakeAuth(configuration);
            return services;
        }


        public static IServiceCollection AddIwentysDatabase(this IServiceCollection services)
        {
            //TODO: replace with normal db
            //services.AddDbContext<IwentysDbContext>(o => o.UseSqlite("Data Source=Iwentys.db"));
            services.AddDbContext<IwentysDbContext>(o => o.UseInMemoryDatabase("Data Source=Iwentys.db"));
            return services;
        }

        public static IServiceCollection AddIwentysServices(this IServiceCollection services)
        {
            if (ApplicationOptions.GithubToken is null)
                services.AddScoped<IGithubApiAccessor, DummyGithubApiAccessor>();
            else
            {
                GithubApiAccessor.Token = ApplicationOptions.GithubToken;
                services.AddScoped<IGithubApiAccessor, GithubApiAccessor>();
            }

            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IStudyGroupRepository, StudyGroupRepository>();
            services.AddScoped<ISubjectActivityRepository, SubjectActivityRepository>();
            services.AddScoped<IAchievementRepository, AchievementRepository>();
            services.AddScoped<IGithubUserDataRepository, GithubUserDataRepository>();
            services.AddScoped<IStudentProjectRepository, StudentProjectRepository>();

            services.AddScoped<IGuildMemberRepository, GuildMemberRepository>();
            services.AddScoped<IGuildRecruitmentRepository, GuildRecruitmentRepository>();
            services.AddScoped<IGuildRepository, GuildRepository>();
            services.AddScoped<IGuildTestTaskSolvingInfoRepository, GuildTestTaskSolvingInfoRepository>();
            services.AddScoped<IGuildTributeRepository, GuildTributeRepository>();
            services.AddScoped<IBarsPointTransactionLogRepository, BarsPointTransactionLogRepository>();
            services.AddScoped<IQuestRepository, QuestRepository>();

            services.AddScoped<GuildRepositoriesScope>();

            services.AddScoped<DatabaseAccessor>();
            services.AddScoped<AchievementProvider>();

            services.AddScoped<AssignmentService>();
            services.AddScoped<BarsPointTransactionLogService>();
            services.AddScoped<CompanyService>();
            services.AddScoped<GithubUserDataService>();
            services.AddScoped<GuildMemberService>();
            services.AddScoped<GuildService>();
            services.AddScoped<GuildRecruitmentService>();
            services.AddScoped<GuildTestTaskService>();
            services.AddScoped<GuildTributeService>();
            services.AddScoped<QuestService>();
            services.AddScoped<StudentService>();
            services.AddScoped<StudyGroupService>();
            services.AddScoped<StudyLeaderboardService>();
            services.AddScoped<TournamentService>();

            return services;
        }

        public static IServiceCollection AddApplicationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            ApplicationOptions.GoogleServiceToken = configuration["GoogleTableCredentials"];
            ApplicationOptions.GithubToken = configuration["GithubToken"];
            ApplicationOptions.TelegramToken = configuration["TelegramToken"];
            ApplicationOptions.SigningSecurityKey = configuration["jwt:SigningSecurityKey"];
            ApplicationOptions.JwtIssuer = configuration["jwt:issuer"];

            ApplicationOptions.IsuClientId = configuration["isu_auth:client_id"];
            ApplicationOptions.IsuClientSecret = configuration["isu_auth:client_secret"];
            ApplicationOptions.IsuRedirection = configuration["isu_auth:redirect_uri"];
            ApplicationOptions.IsuAuthUrl = configuration["isu_auth:isu_auth_url"];
            return services;
        }

        public static IServiceCollection AddIwentysTokenFactory(this IServiceCollection services, IConfiguration configuration)
        {
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

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return services;
        }

        public static IServiceCollection AddIwentysLogging(this IServiceCollection services, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.RollingFile("Logs/iwentys-{Date}.log")
                .CreateLogger();

            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

            return services;
        }

        //TODO: Temp fix for CORS
        public static IServiceCollection AddIwentysCorsHack(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
            return services;
        }
    }
}
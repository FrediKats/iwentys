using Iwentys.Core.Services;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories;
using Iwentys.Database.Repositories.Achievements;
using Iwentys.Endpoints.Shared.Auth;
using Iwentys.Features.Achievements;
using Iwentys.Integrations.GithubIntegration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;

namespace Iwentys.Endpoints.Shared
{
    public static class AspStartupExtensions
    {
        public static IServiceCollection AddIwentysDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IwentysDbContext>(o => o.UseSqlite("Data Source=Iwentys.db"));
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

            services.AddScoped<AppState>();

            services.AddScoped<StudentRepository>();
            services.AddScoped<IAchievementRepository, AchievementRepository>();

            services.AddScoped<DatabaseAccessor>();
            services.AddScoped<AchievementProvider>();

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

            return services;
        }

        public static IServiceCollection AddIwentysLogging(this IServiceCollection services, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.RollingFile("Logs/iwentys-{Date}.log", LogEventLevel.Verbose)
                .CreateLogger();

            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

            return services;
        }
    }
}
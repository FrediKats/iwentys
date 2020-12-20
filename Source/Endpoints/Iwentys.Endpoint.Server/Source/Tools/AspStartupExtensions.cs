using Iwentys.Common.Databases;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Guilds;
using Iwentys.Database.Repositories.Study;
using Iwentys.Database.Tools;
using Iwentys.Endpoint.Server.Source.Tokens;
using Iwentys.Features.Achievements;
using Iwentys.Features.Achievements.Domain;
using Iwentys.Features.Assignments;
using Iwentys.Features.Companies;
using Iwentys.Features.Economy;
using Iwentys.Features.Gamification;
using Iwentys.Features.GithubIntegration;
using Iwentys.Features.Guilds;
using Iwentys.Features.Guilds.Repositories;
using Iwentys.Features.Newsfeeds;
using Iwentys.Features.Quests;
using Iwentys.Features.Students;
using Iwentys.Features.Study;
using Iwentys.Features.Study.Repositories;
using Iwentys.Features.Voting;
using Iwentys.Integrations.GithubIntegration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Iwentys.Endpoint.Server.Source.Tools
{
    public static class AspStartupExtensions
    {
        public static IServiceCollection AddIwentysDatabase(this IServiceCollection services)
        {
            //TODO: replace with normal db
            //services.AddDbContext<IwentysDbContext>(o => o.UseSqlite("Data Source=Iwentys.db"));
            services
                .AddDbContext<IwentysDbContext>(o => o
                    .UseLazyLoadingProxies()
                    .UseInMemoryDatabase("Data Source=Iwentys.db"));
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

            services.AddIwentysAchievementFeatureServices();
            services.AddIwentysAssignmentFeatureServices();
            services.AddIwentysCompanyFeatureServices();
            services.AddIwentysEconomyFeatureServices();
            services.AddIwentysGamificationFeatureServices();
            services.AddIwentysGithubIntegrationFeatureServices();

            services.AddScoped<IGuildRepository, GuildRepository>();
            services.AddIwentysGuildFeatureServices();

            services.AddIwentysNewsfeedFeatureServices();
            services.AddIwentysQuestFeatureServices();
            services.AddIwentysStudentsFeatureServices();
            
            services.AddScoped<ISubjectActivityRepository, SubjectActivityRepository>();
            services.AddIwentysStudyFeatureServices();
            services.AddIwentysVotingFeatureServices();

            services.AddScoped<DatabaseAccessor>();
            services.AddScoped<AchievementProvider>();

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
        public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services)
            where TContext : DbContext
        {
            services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
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
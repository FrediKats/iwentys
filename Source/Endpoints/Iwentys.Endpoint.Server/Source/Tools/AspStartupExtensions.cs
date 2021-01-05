using Iwentys.Common.Databases;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Study;
using Iwentys.Database.Tools;
using Iwentys.Endpoint.Server.Source.IdentityAuth;
using Iwentys.Endpoint.Server.Source.Options;
using Iwentys.Endpoint.Server.Source.Tokens;
using Iwentys.Features.Achievements;
using Iwentys.Features.Achievements.Domain;
using Iwentys.Features.Assignments;
using Iwentys.Features.Companies;
using Iwentys.Features.Economy;
using Iwentys.Features.Gamification;
using Iwentys.Features.GithubIntegration;
using Iwentys.Features.Guilds;
using Iwentys.Features.Guilds.Tournaments;
using Iwentys.Features.Guilds.Tributes;
using Iwentys.Features.Newsfeeds;
using Iwentys.Features.PeerReview;
using Iwentys.Features.Quests;
using Iwentys.Features.Raids;
using Iwentys.Features.Study;
using Iwentys.Features.Study.Repositories;
using Iwentys.Features.Study.SubjectAssignments;
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
        public static IServiceCollection AddIwentysOptions(this IServiceCollection services, IConfiguration configuration)
        {
            var token = TokenApplicationOptions.Load(configuration);
            
            return services
                .AddSingleton(IsuApplicationOptions.Load(configuration))
                .AddSingleton(token)
                .AddSingleton(JwtApplicationOptions.Load(configuration))
                .AddSingleton(new GithubApiAccessorOptions {Token = token.GithubToken})
                .AddSingleton(new ApplicationOptions());
        }

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
            //TODO: replace
            services.AddScoped<IGithubApiAccessor, DummyGithubApiAccessor>();
            //services.AddScoped<IGithubApiAccessor, GithubApiAccessor>();
            services.AddScoped<DatabaseAccessor>();
            services.AddScoped<AchievementProvider>();
            
            
            services.AddIwentysAchievementFeatureServices();
            services.AddIwentysAssignmentFeatureServices();
            services.AddIwentysCompanyFeatureServices();
            services.AddIwentysEconomyFeatureServices();
            services.AddIwentysGamificationFeatureServices();
            services.AddIwentysGithubIntegrationFeatureServices();

            services.AddIwentysGuildFeatureServices();
            services.AddIwentysGuildTournamentFeatureServices();

            services.AddIwentysTributesFeatureServices();
            services.AddIwentysNewsfeedFeatureServices();
            services.AddIwentysPeerReviewFeatureServices();
            services.AddIwentysQuestFeatureServices();
            
            services.AddScoped<ISubjectActivityRepository, SubjectActivityRepository>();
            services.AddIwentysStudyFeatureServices();
            services.AddIwentysSubjectAssignmentFeatureServices();

            services.AddIwentysVotingFeatureServices();
            services.AddIwentysRaidFeatureServices();

            return services;
        }

        public static IServiceCollection AddIwentysTokenFactory(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOptions = JwtApplicationOptions.Load(configuration);
            var signingKey = new SigningSymmetricKey(jwtOptions.SigningSecurityKey);
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
                        ValidIssuer = jwtOptions.JwtIssuer,
                        ValidAudience = jwtOptions.JwtIssuer,
                        IssuerSigningKey = signingKey.GetKey()
                    };
                });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return services;
        }

        public static IServiceCollection AddIwentysLogging(this IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.RollingFile("Logs/iwentys-{Date}.log")
                .CreateLogger();

            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

            return services;
        }

        //TODO: Temp fix for CORS
        public static IServiceCollection AddIwentysCorsHack(this IServiceCollection services)
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

        public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services)
            where TContext : DbContext
        {
            services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
            return services;
        }

        public static IServiceCollection AddLegacyIdentityAuth(this IServiceCollection services)
        {
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();
            
            return services;
        }
    }
}
using Iwentys.Domain.Gamification;
using Iwentys.Domain.GithubIntegration;
using System;
using Iwentys.AccountManagement;
using Iwentys.DataAccess;
using Iwentys.Gamification;
using Iwentys.GithubIntegration;
using Iwentys.Guilds;
using Iwentys.PeerReview;
using Iwentys.Study;
using Iwentys.SubjectAssignments;
using Iwentys.WebService.Application;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Iwentys.Endpoints.Api
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIwentysDatabase(this IServiceCollection services)
        {
            //FYI: need to replace with normal db after release
            services
                .AddDbContext<IwentysDbContext>(o => o
                    .UseLazyLoadingProxies()
                    .UseInMemoryDatabase("Data Source=Iwentys.db"));
            return services;
        }

        public static IServiceCollection EnableExceptional(this IServiceCollection services)
        {
            return services
                .AddDatabaseDeveloperPageExceptionFilter()
                .AddExceptional(settings => { settings.Store.ApplicationName = "Samples.AspNetCore"; });
        }

        public static IServiceCollection AddIwentysModules(this IServiceCollection services)
        {
            services
                .AddAccountManagementModule()
                .AddGamificationModule()
                .AddGuildModule()
                .AddPeerReviewModule()
                .AddStudyModule()
                .AddSubjectAssignmentsModule();

            return services;
        }

        public static IServiceCollection AddIwentysServices(this IServiceCollection services)
        {
            //FYI: replace after release
            services.AddScoped<IGithubApiAccessor, DummyGithubApiAccessor>();
            //services.AddScoped<IGithubApiAccessor, GithubApiAccessor>();
            services.AddScoped<AchievementProvider>();
            services.AddScoped<GithubIntegrationService>();

            return services;
        }

        public static IServiceCollection AddIwentysMediatorHandlers(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ScheduleController).Assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionPipeline<,>));
            return services;
        }

        public static IServiceCollection AddIwentysOptions(this IServiceCollection services, IConfiguration configuration)
        {
            TokenApplicationOptions token = TokenApplicationOptions.Load(configuration);

            return services
                .AddSingleton(token)
                .AddSingleton(new GithubApiAccessorOptions { Token = token.GithubToken })
                .AddSingleton(new ApplicationOptions());
        }

        public static IServiceCollection AddAutoMapperConfig(this IServiceCollection services)
        {
            return services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        //FYI: Need to rework CORS after release
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

        public static IServiceCollection AddIwentysLogging(this IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.File("Logs/iwentys-{Date}.log")
                .CreateLogger();

            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

            return services;
        }
    }
}
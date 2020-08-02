using System;
using Iwentys.Core.GithubIntegration;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Core.Services.Implementations;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Database.Repositories.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddControllers();
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

            services.AddScoped<DatabaseAccessor>();

            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IGuildService, GuildService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ITournamentService, TournamentService>();
            services.AddScoped<IBarsPointTransactionLogService, BarsPointTransactionLogService>();
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
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
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
            services.AddControllers();

            services.AddDbContext<IwentysDbContext>(o => o.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            //TODO: replace with GithubApiAccessor implementation
            services.AddScoped<IGithubApiAccessor, DummyGithubApiAccessor>();

            services.AddScoped<IUserProfileRepository, UserProfileRepository>();
            services.AddScoped<IGuildProfileRepository, GuildProfileRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<ITournamentRepository, TournamentRepository>();

            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddScoped<IGuildProfileService, GuildProfileService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ITournamentService, TournamentService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
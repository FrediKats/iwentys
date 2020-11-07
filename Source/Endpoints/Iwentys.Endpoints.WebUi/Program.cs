using System;
using System.Linq;
using System.Security.Claims;
using Iwentys.Endpoints.Shared.BackgroundServices;
using Iwentys.Features.StudentFeature;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Iwentys.Endpoints.WebUi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .UseSerilog()
                .ConfigureServices(s => s.AddHostedService<MarkUpdateBackgroundService>())
                .ConfigureServices(s => s.AddHostedService<GithubUpdateBackgroundService>());
        }
    }

    //TODO: tmp solution
    public static class HttpContextExtensions
    {
        public static int GetUserId(this IHttpContextAccessor httpContextAccessor)
        {
            var value = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.UserData)?.Value;
            if (string.IsNullOrEmpty(value))
                return 228617;
            return Int32.Parse(value);
        }

        public static AuthorizedUser GetAuthUser(this IHttpContextAccessor httpContextAccessor)
        {
            return AuthorizedUser.DebugAuth(GetUserId(httpContextAccessor));
        }

    }
}
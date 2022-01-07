using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Iwentys.Endpoints.Api.Areas.Identity.IdentityHostingStartup))]
namespace Iwentys.Endpoints.Api.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}
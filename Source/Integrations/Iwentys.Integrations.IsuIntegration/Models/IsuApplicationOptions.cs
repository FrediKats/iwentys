using Microsoft.Extensions.Configuration;

namespace Iwentys.Integrations.IsuIntegration.Models
{
    public class IsuApplicationOptions
    {
        public string IsuClientId { get; set; }
        public string IsuClientSecret { get; set; }
        public string IsuRedirection { get; set; }
        public string IsuAuthUrl { get; set; }

        public static IsuApplicationOptions Load(IConfiguration configuration)
        {
            return new IsuApplicationOptions()
            {
                IsuClientId = configuration["isu_auth:client_id"],
                IsuClientSecret = configuration["isu_auth:client_secret"],
                IsuRedirection = configuration["isu_auth:redirect_uri"],
                IsuAuthUrl = configuration["isu_auth:isu_auth_url"]
            };
        }
    }
}
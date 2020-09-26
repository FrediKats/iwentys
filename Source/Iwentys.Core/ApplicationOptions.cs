using System;
using Microsoft.Extensions.Configuration;

namespace Iwentys.Core
{
    public static class ApplicationOptions
    {
        public static string GoogleServiceToken { get; set; }
        public static string GithubToken { get; set; }
        public static string TelegramToken { get; set; }
        public static string SigningSecurityKey { get; set; }
        public static string JwtIssuer { get; set; }

        public static string IsuClientId { get; set; }
        public static string IsuClientSecret { get; set; }
        public static string IsuRedirection { get; set; }
        public static string IsuAuthUrl { get; set; }

        public static TimeSpan DaemonUpdateInterval = TimeSpan.FromHours(1);

        public static void Load(IConfiguration configuration)
        {
            GoogleServiceToken = configuration["GoogleTableCredentials"];
            GithubToken = configuration["GithubToken"];
            TelegramToken = configuration["TelegramToken"];
            SigningSecurityKey = configuration["jwt:SigningSecurityKey"];
            JwtIssuer = configuration["jwt:issuer"];

            IsuClientId = configuration["isu_auth:client_id"];
            IsuClientSecret = configuration["isu_auth:client_secret"];
            IsuRedirection = configuration["isu_auth:redirect_uri"];
            IsuAuthUrl = configuration["isu_auth:isu_auth_url"];
        }
    }
}
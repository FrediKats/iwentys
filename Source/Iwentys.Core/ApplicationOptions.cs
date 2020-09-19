using System;

namespace Iwentys.Core
{
    public static class ApplicationOptions
    {
        public static string GoogleServiceToken { get; set; }
        public static string GithubToken { get; set; }
        public static string TelegramToken { get; set; }
        public static string SigningSecurityKey { get; set; }
        public static string JwtIssuer { get; set; }

        public static TimeSpan DaemonUpdateInterval = TimeSpan.FromHours(1);
    }
}
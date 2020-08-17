using System;

namespace Iwentys.Core
{
    public static class ApplicationOptions
    {
        public static string GoogleServiceToken { get; set; }
        public static string GithubToken { get; set; }
        //TODO: move to config
        public static string SigningSecurityKey { get; set; } = "0d5b3235a8b403c3dab9c3f4f65c07fcalskd234n1k41230";


        public static TimeSpan DaemonUpdateInterval = TimeSpan.FromHours(1);
    }
}
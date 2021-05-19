using System;

namespace Iwentys.Infrastructure.Configuration.Options
{
    public class ApplicationOptions
    {
        public TimeSpan DaemonUpdateInterval = TimeSpan.FromHours(1);
    }
}
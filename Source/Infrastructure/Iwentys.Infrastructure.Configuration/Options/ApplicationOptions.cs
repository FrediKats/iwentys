using System;

namespace Iwentys.Infrastructure.Options
{
    public class ApplicationOptions
    {
        public TimeSpan DaemonUpdateInterval = TimeSpan.FromHours(1);
    }
}
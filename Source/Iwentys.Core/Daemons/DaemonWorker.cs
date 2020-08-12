using System;
using System.Threading.Tasks;

namespace Iwentys.Core.Daemons
{
    public abstract class DaemonWorker
    {
        public void Notify()
        {
            if (DateTime.UtcNow < _lastCheck + _checkInterval)
                return;

            lock (_lock)
            {
                if (DateTime.UtcNow > _lastCheck + _checkInterval)
                {
                    _lastCheck = DateTime.UtcNow;
                    Task.Run(Execute);
                }
            }
        }

        private DateTime _lastCheck;
        private readonly TimeSpan _checkInterval;
        private readonly object _lock = new object();

        protected DaemonWorker(TimeSpan checkInterval)
        {
            _checkInterval = checkInterval;
            _lastCheck = DateTime.MinValue;
        }

        protected abstract void Execute();
    }
}
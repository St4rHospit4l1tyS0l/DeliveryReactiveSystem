using System;

namespace Drs.Infrastructure.Time
{
    public class IdleTimeInfo
    {
        public DateTime LastInputTime { get; internal set; }

        public TimeSpan IdleTime { get; internal set; }

        public int SystemUptimeMilliseconds { get; internal set; }
    }
}
using System;
using System.Runtime.InteropServices;

namespace Drs.Infrastructure.Time
{
    public static class IdleTimeDetector
    {
        [DllImport("user32.dll")]
        static extern bool GetLastInputInfo(ref LastInputInfo plii);

        public static IdleTimeInfo GetIdleTimeInfo()
        {
            var systemUptime = Environment.TickCount;
            var idleTicks = 0;

            var lastInputInfo = new LastInputInfo();
            lastInputInfo.CbSize = (uint)Marshal.SizeOf(lastInputInfo);
            lastInputInfo.DwTime = 0;

            if (!GetLastInputInfo(ref lastInputInfo))
            {
                return new IdleTimeInfo
                {
                    LastInputTime = DateTime.Now.AddMilliseconds(-1 * idleTicks),
                    IdleTime = new TimeSpan(0, 0, 0, 0, idleTicks),
                    SystemUptimeMilliseconds = systemUptime,
                };
            }

            var lastInputTicks = (int)lastInputInfo.DwTime;
            idleTicks = systemUptime - lastInputTicks;

            return new IdleTimeInfo
            {
                LastInputTime = DateTime.Now.AddMilliseconds(-1 * idleTicks),
                IdleTime = new TimeSpan(0, 0, 0, 0, idleTicks),
                SystemUptimeMilliseconds = systemUptime,
            };
        }
    }
}
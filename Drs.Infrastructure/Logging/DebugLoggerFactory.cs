using System;

namespace Drs.Infrastructure.Logging
{
    public class DebugLoggerFactory : ILoggerFactory
    {
        public ILog Create(Type type)
        {
            return new DebugLogger(type.Name);
        }
    }
}

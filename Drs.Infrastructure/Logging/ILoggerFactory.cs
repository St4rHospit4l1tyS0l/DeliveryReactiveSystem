using System;

namespace Drs.Infrastructure.Logging
{
    public interface ILoggerFactory
    {
        ILog Create(Type type);
    }
}

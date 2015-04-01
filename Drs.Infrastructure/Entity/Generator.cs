using System;

namespace Drs.Infrastructure.Entity
{
    public static class Generator
    {
        public static long GenerateUniqueId()
        {
            var buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }
    }
}

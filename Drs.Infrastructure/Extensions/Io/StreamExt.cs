using System;
using System.IO;

namespace Drs.Infrastructure.Extensions.Io
{
    public static class StreamExt
    {
        public static void SafeDispose(this Stream stream)
        {
            try
            {
                if(stream != null)
                    stream.Dispose();
            }
            catch (Exception)
            {
                return;
            }
        }

        public static void SaveToFile(this Stream stream, string fullFileName)
        {
            using (var fileStream = new FileStream(fullFileName, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(fileStream);
                fileStream.Flush(true);
            }

            stream.SafeDispose();
        }
    }
}

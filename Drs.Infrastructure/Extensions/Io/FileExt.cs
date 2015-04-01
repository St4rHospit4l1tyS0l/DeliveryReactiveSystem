using System;
using System.IO;
using System.Threading;

namespace Drs.Infrastructure.Extensions.Io
{
    public static class FileExt
    {
        public static void ForceDeleteFile(string sFile, int iTries = 3)
        {
            var isDone = false;
            while (iTries >= 0 && isDone == false)
            {
                try
                {
                    File.Delete(sFile);
                    isDone = true;
                }
                catch (Exception)
                {
                    isDone = false;
                    Thread.Sleep(250);
                }
                iTries--;
            }

        }
    }
}

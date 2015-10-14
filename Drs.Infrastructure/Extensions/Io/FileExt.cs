using System;
using System.IO;
using System.Security.Cryptography;
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

        public static string GetChecksum(this string filePath)
        {
            HashAlgorithm algorithm = new MD5CryptoServiceProvider();
            using (var stream = new BufferedStream(File.OpenRead(filePath), 131072))
            {
                var hash = algorithm.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }
    }
}

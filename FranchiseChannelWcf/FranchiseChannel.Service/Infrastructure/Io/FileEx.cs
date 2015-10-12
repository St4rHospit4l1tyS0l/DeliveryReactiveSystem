using System;
using System.IO;
using System.Security.Cryptography;

namespace FranchiseChannel.Service.Infrastructure.Io
{
    public static class FileEx
    {
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

using System;
using System.Security.Cryptography;
using System.Text;

namespace Drs.Infrastructure.Crypto
{
    public static class HashMd5
    {
        public static string CreateHash(this string plain, out string salt)
        {
            salt = Guid.NewGuid().ToString();
            var data = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(salt + plain));
            return Convert.ToBase64String(data);
        }


        public static bool IsEqualHash(this string plain, string salt, string hashed)
        {
            var data = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(salt + plain));
            return Convert.ToBase64String(data) == hashed;
        }



        public static string Hash(this string plain, string salt)
        {
            var data = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(salt + plain));
            return Convert.ToBase64String(data);
        }
    }
}

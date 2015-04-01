using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Drs.Infrastructure.Crypto
{
    public class Cypher
    {

        public static string Encrypt(String plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            var keyBytes = new Rfc2898DeriveBytes(CryptoConstants.PASSWORD_HASH, Encoding.ASCII.GetBytes(CryptoConstants.SALT_KEY)).GetBytes(256/8);

            using (var symmetricKey = new RijndaelManaged { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros })
            {
                using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(CryptoConstants.VI_KEY)))
                {
                    byte[] cipherTextBytes;
                    using (var memoryStream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                            cryptoStream.FlushFinalBlock();
                            cipherTextBytes = memoryStream.ToArray();
                            cryptoStream.Close();
                        }
                        memoryStream.Close();
                    }
                    return Convert.ToBase64String(cipherTextBytes);                    
                }
            }
        }


        public static string Decrypt(String encryptedText)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = new Rfc2898DeriveBytes(CryptoConstants.PASSWORD_HASH, Encoding.ASCII.GetBytes(CryptoConstants.SALT_KEY)).GetBytes(256 / 8);

            using (var symmetricKey = new RijndaelManaged { Mode = CipherMode.CBC, Padding = PaddingMode.None })
            {
                using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(CryptoConstants.VI_KEY)))
                {
                    using (var memoryStream = new MemoryStream(cipherTextBytes))
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            var plainTextBytes = new byte[cipherTextBytes.Length];
                            var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                            memoryStream.Close();
                            cryptoStream.Close();
                            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
                            
                        }
                    }
                }
            }    
        }
    }
}

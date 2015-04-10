using System.Web.Script.Serialization;
using Drs.Infrastructure.Crypto;

namespace Drs.Infrastructure.Extensions.Json
{
    public static class JavaScriptSerializerExt
    {
        public static string Serialize<T>(this T obj)
        {
            return new JavaScriptSerializer().Serialize(obj);
        }

        public static string SerializeAndEncrypt<T>(this T obj)
        {
            return Cypher.Encrypt(new JavaScriptSerializer().Serialize(obj));
        }

        public static T Deserialize<T>(this string data)
        {
            return new JavaScriptSerializer().Deserialize<T>(data);
        }

        public static T DeserializeAndDecrypt<T>(this string data)
        {
            return new JavaScriptSerializer().Deserialize<T>(Cypher.Decrypt(data));
        }

    }
}

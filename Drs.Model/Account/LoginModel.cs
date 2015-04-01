using System.Runtime.Serialization;

namespace Drs.Model.Account
{
    [DataContract]
    public class LoginModel
    {
        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Password { get; set; }
    }
}

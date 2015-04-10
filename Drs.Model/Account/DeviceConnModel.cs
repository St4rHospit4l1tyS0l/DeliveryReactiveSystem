using System.Collections.Generic;

namespace Drs.Model.Account
{
    public class DeviceConnModel
    {
        public List<string> LstClients { get; set; }
        public List<string> LstServers { get; set; }
        public string ActivationCode { get; set; }

        public DeviceConnModel()
        {
            LstClients = new List<string>();
            LstServers = new List<string>();
        }
    }
}
using System.Collections.Generic;
using Drs.Infrastructure.Model;

namespace Drs.Model.Account
{
    public class DeviceInfoModel
    {
        public List<ConnectionFullModel> LstClients { get; set; }
        public List<ConnectionFullModel> LstServers { get; set; }

        public DeviceInfoModel()
        {
            LstClients = new List<ConnectionFullModel>();
            LstServers = new List<ConnectionFullModel>();
        }
    }
}

using System;
using System.ServiceModel;
using FranchiseChannel.Service.Model;

namespace FranchiseChannel.Service.Sync
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISyncService" in both code and config file together.
    [ServiceContract]
    public interface ISyncService
    {
        [OperationContract]
        ResponseMessageFc QueryForFiles(Guid uidVersion);
    }
}

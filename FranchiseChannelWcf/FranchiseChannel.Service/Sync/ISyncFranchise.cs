using System;
using System.ServiceModel;
using FranchiseChannel.Service.Model;

namespace FranchiseChannel.Service.Sync
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISyncFranchise" in both code and config file together.
    [ServiceContract]
    public interface ISyncFranchise
    {
        [OperationContract]
        ResponseMessageFc QueryForFiles(Guid uidVersion);

        [OperationContract]
        ResponseMessageFcUnSync GetUnSyncListOfFiles(Guid uidVersion);

        [OperationContract]
        ResponseMessageFileSync GetFileByName(RequestMessageFileSync request);
    }
}

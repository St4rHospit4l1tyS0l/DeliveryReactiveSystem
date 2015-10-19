using System;
using System.ServiceModel;
using Drs.Model.Sync;

namespace ManagementCallCenter.Sync
{
    [ServiceContract]
    public interface ISyncServerSvc
    {
        [OperationContract]
        ResponseMessageServerFileSync GetFileByName(RequestMessageServerFileSync request);

        [OperationContract]
        void SetFranchiseVersionTerminalOk(int franchiseId, String sVersion, string sMaInfo);

    }
}

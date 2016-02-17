using System.ServiceModel;

namespace Drs.VirtualPos.Ws.Sync
{
    [ServiceContract]
    public interface IVirtualPosConnect
    {
        [OperationContract]
        string GetData(int value);
    }
}

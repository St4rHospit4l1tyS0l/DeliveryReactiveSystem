using System.ServiceModel;
using Drs.Model.Shared;
using Drs.Model.Store;

namespace ManagementCallCenter.Store
{
    [ServiceContract]
    public interface IStoreSvc
    {
        [OperationContract]
        ResponseMessageShared UpdateOrderStatus(long orderId, string referenceId, string comments, string status);

        [OperationContract]
        ResponseStoreOrdersMessage GetAllInProgressOrdersByStore(int storeId);
    }
}

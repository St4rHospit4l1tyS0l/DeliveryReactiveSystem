using System;
using Drs.Model.Shared;
using Drs.Model.Store;
using Drs.Service.Store;

namespace ManagementCallCenter.Store
{

    public class StoreSvc : IStoreSvc
    {
        private readonly IStoreService _service;

        public StoreSvc(IStoreService service)
        {
            _service = service;
        }


        public ResponseMessageShared UpdateOrderStatus(long orderId, string referenceId, string comments, string status)
        {
            try
            {
                return _service.UpdateOrderStatus(orderId, referenceId, comments, status);
            }
            catch (Exception ex)
            {
                return new ResponseMessageShared { IsSuccess = false, Message = ex.Message };
            }
        }

        public ResponseStoreOrdersMessage GetAllInProgressOrdersByStore(int storeId)
        {
            try
            {
                return _service.GetAllInProgressOrdersByStore(storeId);
            }
            catch (Exception ex)
            {
                return new ResponseStoreOrdersMessage { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}

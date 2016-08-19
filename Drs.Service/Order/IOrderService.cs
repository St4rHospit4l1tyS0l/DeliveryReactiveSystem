using System.Collections.Generic;
using Drs.Model.Address;
using Drs.Model.Client;
using Drs.Model.Menu;
using Drs.Model.Order;
using Drs.Model.Shared;

namespace Drs.Service.Order
{
    public interface IOrderService
    {
        ResponseMessageData<PhoneModel> SavePhone(PhoneModel model, bool bHasToDispose = true);
        ResponseMessageData<ClientInfoModel> SaveClient(ClientInfoModel model);
        ResponseMessageData<bool> RemoveRelPhoneClient(ClientPhoneModel model);
        ResponseMessageData<bool> RemoveRelPhoneAddress(AddressPhoneModel model);
        ResponseMessageData<PosCheck> SavePosCheck(PosCheck model);
        ResponseMessageData<PropagateOrderModel> PosOrderById(int posOrderId);
        ResponseMessageData<PosCheck> CalculatePrices(string phone);
        ResponseMessageData<LastOrderInfoModel> LastNthOrdersByPhone(string phone);
    }
}

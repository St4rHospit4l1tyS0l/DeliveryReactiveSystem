using Drs.Model.Address;
using Drs.Model.Client;
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
        ResponseMessageData<PosCheck> LastOrderByPhone(string phone);
    }
}

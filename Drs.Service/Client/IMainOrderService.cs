using System;
using Drs.Model.Address;
using Drs.Model.Constants;
using Drs.Model.Order;
using Drs.Model.Shared;

namespace Drs.Service.Client
{
    public interface IMainOrderService
    {
        void ResetAndCreateNewOrder();
        void ProcessPhone(ListItemModel itemModel);
        void SavePhoneInformation();

        event Action<PhoneModel> PhoneChanged;
        event Action<FranchiseInfoModel> FranchiseChanged;
        event Action<ClientInfoGrid> ClientChanged;
        event Action<AddressInfoGrid> AddressChanged;
        event Action<PosCheck> PosOrderChanged;
        event Action<OrderModelDto> SendOrderToStoreStatusChanged;

        void ProcessFranchise(FranchiseInfoModel franchiseInfo);
        Func<ClientFlags.ValidateOrder, ResponseMessage> ValidateModel { get; }
        void ProcessClient(ClientInfoGrid clientInfoModel);
        void ProcessAddress(AddressInfoGrid infoModel);
        //IReactiveList<ClientInfoGrid> LstClientInfo { get; }
        //IReactiveList<AddressInfoGrid> LstAddressInfo { get; }
        OrderModel OrderModel { get; }
        void ProcessPosOrder(PosCheck posCheck);
        void SavePosOrder();
        void SendOrderToStore(OrderDetails orderDetails);
    }
}

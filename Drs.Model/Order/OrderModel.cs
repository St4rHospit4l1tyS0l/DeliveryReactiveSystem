using System.Linq;
using Drs.Model.Address;
using Drs.Model.Store;
using ReactiveUI;

namespace Drs.Model.Order
{
    public class OrderModel
    {
        public OrderModel()
        {
            PhoneInfo = new PhoneModel();
            Franchise = new FranchiseInfoModel();
            LstClientInfo = new ReactiveList<ClientInfoGrid>();
            LstAddressInfo = new ReactiveList<AddressInfoGrid>();
        }
        public PhoneModel PhoneInfo { get; set; }
        public FranchiseInfoModel Franchise { get; set; }
        public IReactiveList<ClientInfoGrid> LstClientInfo { get; set; }
        public IReactiveList<AddressInfoGrid> LstAddressInfo { get; set; }
        public PosCheck PosCheck { get; set; }
        public StoreModel StoreModel { get; set; }
        public string Username { get; set; }
        public OrderDetails OrderDetails { get; set; }

        public OrderModelDto ToDto()
        {
            return new OrderModelDto
            {
                PhoneId = PhoneInfo.PhoneId,
                FranchiseCode = Franchise.Code,
                ClientId = LstClientInfo.Where(e => e.IsSelected && e.ClientInfo.ClientId.HasValue).Select(e => e.ClientInfo.ClientId).Single(),
                AddressInfo = LstAddressInfo.Where(e => e.IsSelected).Select(e => e.AddressInfo).Single(),
                PosOrder = PosCheck,
                Username = Username,
                StoreModel = StoreModel,
                OrderDetails = OrderDetails,
            };
        }
    }
}

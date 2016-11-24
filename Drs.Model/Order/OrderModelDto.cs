using Drs.Model.Address;
using Drs.Model.Store;

namespace Drs.Model.Order
{
    public class OrderModelDto
    {
        public long PhoneId { get; set; }
        public string FranchiseCode { get; set; }
        public long? ClientId { get; set; }
        public AddressInfoModel AddressInfo { get; set; }
        public PosCheck PosOrder { get; set; }
        public StoreModel Store { get; set; }
        public int FranchiseId { get; set; }
        public string Username { get; set; }
        public string UserId { get; set; }
        public long OrderToStoreId { get; set; }
        public bool HasError { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public bool IsAlreadyOnStore { get; set; }
        public ClientInfoModel ClientInfo { get; set; }
        public string Phone { get; set; }
        public OrderDetails OrderDetails { get; set; }
    }
}
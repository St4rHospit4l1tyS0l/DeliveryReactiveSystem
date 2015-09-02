namespace Drs.Model.Store
{
    public class StoreUpModel
    {
        public int FranchiseStoreId { get; set; }
        public string Name { get; set; }
        public int FranchiseId { get; set; }
        public int AddressId { get; set; }
        public string WsAddress { get; set; }
    }
}
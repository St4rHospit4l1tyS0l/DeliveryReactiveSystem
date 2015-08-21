namespace Drs.Repository.Entities.Metadata
{
    public class StoreInfoDto
    {
        public int FranchiseStoreId { get; set; }
        public string Name { get; set; }
        public string FranchiseName { get; set; }
        public string Address { get; set; }
        public string WsAddress { get; set; }
        public string UserNameIns { get; set; }

        public static ViewStoreInfoModel ToDto(StoreInfoDto data)
        {
            return new ViewStoreInfoModel
            {
                FranchiseStoreId = data.FranchiseStoreId,
                Name = data.Name
            };
        }
    }
}
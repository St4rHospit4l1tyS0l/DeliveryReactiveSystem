namespace Drs.Model.Report
{
    public class PosOrderInfoModel
    {
        public long OrderToStoreId { get; set; }
        public long ItemId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
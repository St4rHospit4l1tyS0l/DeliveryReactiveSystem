using System;

namespace Drs.Model.Store
{
    public class TrackOrderModel
    {
        public long OrderToStoreId { get; set; }
        public string AtoOrderId { get; set; }
        public string LastStatus { get; set; }
        public DateTime? PromiseTime { get; set; }
        public int StoreId { get; set; }
        public int FailedStatusCounter { get; set; }
    }
}

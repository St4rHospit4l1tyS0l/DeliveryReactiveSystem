using System;

namespace Drs.Model.Order
{
    public class LastOrderInfoModel
    {
        public int PosOrderId { get; set; }
        public string ClientName { get; set; }
        public string FranchiseName { get; set; }
        public string StoreName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
    }
}
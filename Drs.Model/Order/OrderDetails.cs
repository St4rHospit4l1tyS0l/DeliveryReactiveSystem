using System;
using Drs.Model.Shared;

namespace Drs.Model.Order
{
    public class OrderDetails
    {
        public int PosOrderStatus { get; set; }
        public string ExtraNotes { get; set; }
        public DateTime PromiseTime { get; set; }
        public ItemCatalog Payment { get; set; }
        public string PosOrderMode { get; set; }
    }
}

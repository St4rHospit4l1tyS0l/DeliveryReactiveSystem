using System;
using Drs.Model.Shared;

namespace Drs.Model.Order
{
    public class OrderDetails
    {
        public int OrderMode { get; set; }
        public string ExtraNotes { get; set; }
        public DateTime PromiseTime { get; set; }
        public ItemCatalog Payment { get; set; }
    }
}

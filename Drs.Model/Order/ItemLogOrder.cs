using System;
using Drs.Model.Catalog;
using Drs.Model.Settings;
using Drs.Model.Shared;

namespace Drs.Model.Order
{
    public class ItemLogOrder
    {
        public long Id { get; set; }
        public DateTime Timestamp { get; set; }

        public string StrTimestamp
        {
            get
            {
                return Timestamp.ToString("yyyy / MM / dd  |  HH:mm:ss");
            }
        }
        public string Status { get; set; }
        public string StatusEx {
            get
            {
                ItemCatalog catalog;
                return CatalogsClientModel.DicOrderStatus.TryGetValue(Status, out catalog) ? catalog.Name : Status;
            }
        }
    }
}
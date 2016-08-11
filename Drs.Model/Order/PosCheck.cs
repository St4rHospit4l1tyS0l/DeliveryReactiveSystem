using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Drs.Model.Shared;

namespace Drs.Model.Order
{
    public class PosCheck : RecordModel
    {
        public List<ItemModel> LstItems { get; set; }
        public double Total { get; set; }
        public double SubTotal { get; set; }
        public double Tax { get; set; }

        public string TotalTx {
            get
            {
                return String.Format("{0:C2}", Total);
            }
        }

        public string SubTotalTx
        {
            get
            {
                return String.Format("{0:C2}", SubTotal);
            }
        }

        public string TaxTx
        {
            get
            {
                return String.Format("{0:C2}", Tax);
            }
        }

        public int CheckId { get; set; }
        public Guid GuidId { get; set; }
        public int? Id { get; set; }

        public string FranchiseCode { get; set; }
        public DateTime OrderDateTime { get; set; }

        public void FixItemParents()
        {
            var lstPosItems = LstItems;

            if(lstPosItems == null)
                return;

            var dictItems = new Dictionary<long, ItemModel>();

            foreach (var item in lstPosItems.OrderBy(e => e.CheckItemId))
            {
                dictItems[item.CheckItemId] = item;

                if (item.ParentId != null)
                {
                    item.Parent = dictItems[item.ParentId.Value];
                }
            }
        }
    }
}

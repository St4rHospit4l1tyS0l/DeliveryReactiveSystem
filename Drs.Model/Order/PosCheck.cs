using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Drs.Model.Shared;
using Newtonsoft.Json;

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
        public long? Id { get; set; }

        public string FranchiseCode { get; set; }
        public DateTime OrderDateTime { get; set; }
        public Dictionary<long, PromoModel> Promos { get; set; }
        public List<PromoModel> LstPromos { get; set; }

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

        public void ConvertToDicPromos()
        {
            if (LstPromos == null || !LstPromos.Any())
                return;

            Promos = LstPromos.ToDictionary(e => (long)e.PromoEntryId, e => new PromoModel
            {
                PromoEntryId = e.PromoEntryId,
                PromoTypeId = e.PromoTypeId,
                LstEntries = JsonConvert.DeserializeObject<List<int>>(e.EntriesIdsSelected)
            });

            LstPromos = null;
        }
    }
}

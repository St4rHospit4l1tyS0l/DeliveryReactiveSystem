using System;
using System.Collections.Generic;
using Drs.Model.Order;

namespace Drs.Model.Track
{
    public class TrackOrderDetailDto : TrackOrderDto
    {
        public DateTime? EndDatetime { get; set; }
        public string StrEndDatetime {
            get
            {
                if (EndDatetime == null)
                    return Constants.SharedConstants.NOT_APPLICABLE;
                return EndDatetime.Value.ToString("yyyy / MM / dd  |  HH:mm:ss");
            }
        }
        public string FranchiseName { get; set; }
        public string Mode { get; set; }
        public string UserTakeOrder { get; set; }
        public IList<ItemPosOrder> LstOrderPos { get; set; }
        public string PromiseTime { get; set; }
        public List<ItemLogOrder> LstOrderLog { get; set; }
        public string WsAddress { get; set; }
        public string StoreErrMsg { get; set; }
    }
}

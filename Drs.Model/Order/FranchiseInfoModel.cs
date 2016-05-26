using System;
using Drs.Model.Shared;

namespace Drs.Model.Order
{
    public class FranchiseInfoModel : RecordModel
    {
        public String Code { get; set; }
        public String Title { get; set; }
        public dynamic DataInfo { get; set; }
        public PropagateOrderModel PropagateOrder { get; set; }
        public string LastConfig { get; set; }
        public string StoresCoverage { get; set; }
        public void CopyTo(FranchiseInfoModel destination)
        {
            destination.Code = Code;
            destination.Title = Title;
            destination.DataInfo = DataInfo;
            destination.LastConfig = LastConfig;
            destination.StoresCoverage = StoresCoverage;
        }
    }
}

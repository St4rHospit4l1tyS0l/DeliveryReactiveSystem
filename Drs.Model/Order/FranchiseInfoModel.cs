using System;
using Drs.Model.Shared;

namespace Drs.Model.Order
{
    public class FranchiseInfoModel : RecordModel
    {
        public String Code { get; set; }
        public String Title { get; set; }
        public dynamic DataInfo { get; set; }
    }
}

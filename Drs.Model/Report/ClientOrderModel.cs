using System;
using System.Collections.Generic;

namespace Drs.Model.Report
{
    public class ClientOrderModel
    {
        public string FranchiseName { get; set; }
        public string FranchiseStoreName { get; set; }
        public string Phone { get; set; }
        public long ClientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long OrderToStoreId { get; set; }
        public string OrderAtoId { get; set; }
        public DateTime FirstDatetime { get; set; }
        public string OrderDateTimeTx
        {
            get
            {
                return FirstDatetime.ToString(Constants.SharedConstants.DATE_TIME_FORMAT);
            }
        }
        public string LastStatus { get; set; }
        public decimal Total { get; set; }
        public List<PosOrderInfoModel> LstPosOrder { get; set; }

        public ClientOrderModel()
        {
            LstPosOrder =new List<PosOrderInfoModel>();
        }

    }
}
using System;

namespace Drs.Model.Store
{
    public class TrackOrderModel
    {
        private DateTime? _promiseTimeDt;
        public long OrderToStoreId { get; set; }
        public string AtoOrderId { get; set; }
        public string LastStatus { get; set; }
        public string PromiseTime { get; set; }
        public int StoreId { get; set; }
        public int FailedStatusCounter { get; set; }

        public DateTime PromiseTimeDt
        {
            get
            {
                if (_promiseTimeDt.HasValue == false)
                    _promiseTimeDt = DateTime.Parse(PromiseTime);

                return _promiseTimeDt.Value;
            }
        }

    }
}

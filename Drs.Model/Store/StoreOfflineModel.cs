using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Drs.Model.Store
{
    public class StoreOfflineModel
    {
        [Required]
        public int FranchiseStoreOffLineId { get; set; }

        [Required]
        public int FranchiseStoreId { get; set; }

        [Required]
        public string UtcStartDateTimeTx { get; set; }

        public DateTime UtcStartDateTime
        {
            get
            {
                if (String.IsNullOrWhiteSpace(UtcStartDateTimeTx))
                    return DateTime.UtcNow;

                return DateTime.ParseExact(UtcStartDateTimeTx, Constants.SharedConstants.DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
            }
        }

        [Required]
        public int Duration { get; set; }

        public string StoreName { get; set; }
        public DateTime UtcStartDateTimeSaved { get; set; }

        public bool IsUndefinedOfflineTime { get; set; }
    }
}
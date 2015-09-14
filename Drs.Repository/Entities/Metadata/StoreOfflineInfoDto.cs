using System;
using Drs.Model.Constants;

namespace Drs.Repository.Entities.Metadata
{
    public class StoreOfflineInfoDto
    {
        public static DateTime UtcTime;

        public int FranchiseStoreOffLineId { get; set; }
        public string DateTimeStart {
            get
            {
                return DateTimeStartInfo.ToString(SharedConstants.DATE_TIME_FORMAT);
            }
        }
        public DateTime DateTimeStartInfo { get; set; }
        public int Duration { get; set; }
        public DateTime DateTimeEndInfo { get; set; }

        public string DateTimeEnd
        {
            get
            {
                return DateTimeEndInfo.ToString(SharedConstants.DATE_TIME_FORMAT);
            }
        }

        public string State
        {
            get
            {
                if (UtcTime >= DateTimeStartInfo && UtcTime <= DateTimeEndInfo)
                    return "OFFL";

                return "ONL";
            }
        }
    }
}
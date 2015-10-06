using System;
using Drs.Model.Constants;

namespace Drs.Repository.Entities.Metadata
{
    public class FranchiseDataVersionInfoDto
    {
        public int FranchiseDataVersionId { get; set; }
        public string Version { get; set; }
        public DateTime TimestampDt { get; set; }
        public String Timestamp {
            get
            {
                return TimestampDt.ToString(SharedConstants.DATE_TIME_FORMAT);
            }
        }
        public int TotalNumberOfFiles { get; set; }
        public int NumberOfFilesDownloaded { get; set; }
        
        public bool IsCompletedBl { get; set; }

        public String IsCompleted
        {
            get
            {
                return IsCompletedBl ? SharedConstants.YES : SharedConstants.NO;
            }
        }


        public DateTime? TimestampCompleteDt { get; set; }

        public String TimestampComplete
        {
            get
            {
                if (TimestampCompleteDt.HasValue == false)
                    return SharedConstants.NOT_APPLICABLE;

                return TimestampCompleteDt.Value.ToString(SharedConstants.DATE_TIME_FORMAT);
            }
        }

        public String UserName { get; set; }
    }
}
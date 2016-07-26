using System;
using Drs.Infrastructure.Extensions;

namespace Drs.Model.Report
{
    public class ReportRequestModel
    {
        private DateTime? _startCalculatedDate;
        private DateTime? _endCalculatedDate;
        public string StartRequestDate { get; set; }

        public string EndRequestDate { get; set; }

        public DateTime StartCalculatedDate
        {
            get
            {
                if (_startCalculatedDate != null)
                    return _startCalculatedDate.Value;
                _startCalculatedDate = StartRequestDate.ExtractDateOrDefault(DateTime.Today);
                return _startCalculatedDate.Value;
            }
        }

        public DateTime EndCalculatedDate
        {
            get
            {
                if (_endCalculatedDate != null)
                    return _endCalculatedDate.Value;

                var dtStart = StartRequestDate.ExtractDateOrDefault(DateTime.Today);
                _endCalculatedDate = String.IsNullOrEmpty(EndRequestDate) ? dtStart : EndRequestDate.ExtractDateOrDefault(dtStart);
                return _endCalculatedDate.Value;

            }
        }
    }
}

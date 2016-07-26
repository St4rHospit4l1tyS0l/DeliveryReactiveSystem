using System;

namespace Drs.Model.Report
{
    public class ReportRequestMonthModel
    {
        private int? _month;
        private int? _year;

        public string RequestDate { get; set; }
        
        public int Month
        {
            get
            {
                if (_month != null)
                    return _month.Value;
                if (String.IsNullOrWhiteSpace(RequestDate))
                    return -1;
                _month = int.Parse(RequestDate.Substring(0,2));
                return _month.Value;
            }
        }
        
        public int Year {
            get
            {
                if (_year != null)
                    return _year.Value;
                if (String.IsNullOrWhiteSpace(RequestDate))
                    return -1;
                _year = int.Parse(RequestDate.Substring(3, 4));
                return _year.Value;
            }
        }
    }
}
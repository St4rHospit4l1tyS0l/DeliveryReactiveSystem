using System;

namespace Drs.Model.Track
{
    public class DailySearchModel
    {
        public DateTime SearchDate { get; set; }
        public int StoreId { get; set; }
        public string AgentId { get; set; }
    }
}
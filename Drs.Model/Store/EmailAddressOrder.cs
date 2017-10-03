using Drs.Infrastructure.Extensions;

namespace Drs.Model.Store
{
    public class EmailAddressOrder
    {
        public string Reference { get; set; }
        public string ExtIntNumber { get; set; }
        public string MainAddress { get; set; }
        public string RegionD { get; set; }
        public string RegionC { get; set; }
        public string RegionB { get; set; }
        public string RegionA { get; set; }
        public string Country { get; set; }

        public string GetInfo()
        {
            return string.Format("{0}, {1}, {2}<br />{3}{4}{5}{6}{7}", MainAddress, ExtIntNumber, Reference,
                StringExt.AddIfNotNull(RegionD), StringExt.AddIfNotNull(RegionC), StringExt.AddIfNotNull(RegionB),
                StringExt.AddIfNotNull(RegionA), StringExt.AddIfNotNull(Country));
        }
    }
}
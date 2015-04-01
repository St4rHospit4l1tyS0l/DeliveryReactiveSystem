using System;

namespace Drs.Model.Address
{
    public class AddressHierarchyInfo
    {
        public String RegionParent { get; set; }
        public String RegionChild { get; set; }
        public String PropertyParent { get; set; }
        public String PropertyChild { get; set; }
    }
}

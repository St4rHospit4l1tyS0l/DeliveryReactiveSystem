using System;

namespace Drs.Model.Address
{
    public class AddressMapInfoModel
    {

        public AddressMapModel Address { get; set; }
        public String PlaceId { get; set; }
        public PositionModel Position { get; set; }
        public int[] CoverageStoreIds { get; set; }
    }
}
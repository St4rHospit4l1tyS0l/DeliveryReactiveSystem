using System.Collections.Generic;

namespace Drs.Model.Shared
{
    public class AddressControlSetting
    {
        public ControlInfoModel MainAddress { get; set; }
        public ControlInfoModel NumExt { get; set; }
        public ControlInfoModel RegionD { get; set; }
        public ControlInfoModel RegionC { get; set; }
        public ControlInfoModel RegionB { get; set; }
        public ControlInfoModel RegionA { get; set; }
        public ControlInfoModel Country { get; set; }
        public ControlInfoModel ZipCode { get; set; }
        public ControlInfoModel Reference { get; set; }

        public List<ControlInfoModel> LstAddressControls { get; private set; }


        public AddressControlSetting()
        {
            LstAddressControls = new List<ControlInfoModel>();
        }


        public void InitList()
        {
            LstAddressControls.Add(Country);
            LstAddressControls.Add(RegionA);
            LstAddressControls.Add(RegionB);
            LstAddressControls.Add(RegionC);
            LstAddressControls.Add(RegionD);
            LstAddressControls.Add(MainAddress);
            LstAddressControls.Add(NumExt);
            LstAddressControls.Add(ZipCode);
            LstAddressControls.Add(Reference);
        }
    }
}

using System.Collections.Generic;
using Drs.Model.Address;
using Drs.Model.Shared;

namespace Drs.Service.Address
{
    public interface IAddressService
    {
        IEnumerable<AddressResponseSearch> SearchHierarchyByZipCode(string zipCode);
        IEnumerable<ListItemModel> SearchByZipCode(string zipCode);
        IEnumerable<ListItemModel> FillNextListByName(string sNextList, int iIdSelected, out string sControlName);
        ResponseMessageData<AddressInfoModel> SaveAddress(AddressInfoModel model);
        IEnumerable<AddressInfoModel> SearchAddressByPhone(string phone);
    }
}

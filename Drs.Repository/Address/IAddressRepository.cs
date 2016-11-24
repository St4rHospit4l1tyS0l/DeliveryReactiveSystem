using System;
using System.Collections.Generic;
using Drs.Model.Address;
using Drs.Model.Shared;
using Drs.Repository.Entities;

namespace Drs.Repository.Address
{
    public interface IAddressRepository : IDisposable
    {
        IDictionary<String, AddressHierarchyInfo> GetAddressHierarchy();
        //AddressResponseSearch SearchByZipCode(IDictionary<string, AddressHierarchyInfo> dicAddressHierarchy, string zipCode);
        CallCenterEntities InnerDbEntities { get;}
        IEnumerable<ListItemModel> SearchByZipCode(string zipCode, int maxResultsOnQuery);
        long SaveAddress(AddressInfoModel model, bool bIsNew);
        long SaveAddressMap(AddressInfoModel model, bool bIsNew);
        IEnumerable<AddressInfoModel> SearchAddressByPhoneId(long phoneId);
    }
}

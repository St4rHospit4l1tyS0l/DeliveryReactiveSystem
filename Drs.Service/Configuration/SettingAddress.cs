using System.Collections.Generic;
using Drs.Model.Address;
using Drs.Repository.Address;

namespace Drs.Service.Configuration
{
    public static class SettingAddress
    {
        public static void Initialize(IAddressRepository repository)
        {
            using (repository)
            {
                DicAddressHierarchy = repository.GetAddressHierarchy();
                
            }
        }

        public static IDictionary<string, AddressHierarchyInfo> DicAddressHierarchy { get; set; }
    }
}

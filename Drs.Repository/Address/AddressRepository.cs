using System.Collections.Generic;
using System.Linq;
using Drs.Model.Address;
using Drs.Model.Order;
using Drs.Model.Shared;
using Drs.Repository.Entities;
using Drs.Repository.Shared;

namespace Drs.Repository.Address
{
    public class AddressRepository : BaseOneRepository, IAddressRepository
    {

        public IDictionary<string, AddressHierarchyInfo> GetAddressHierarchy()
        {
            return DbEntities.AddressHierarchy.ToDictionary(e => e.RegionParent, e => new AddressHierarchyInfo
            {
                PropertyChild = e.PropertyChild,
                PropertyParent = e.PropertyParent,
                RegionChild = e.RegionChild,
                RegionParent = e.RegionParent
            });
        }

        public CallCenterEntities InnerDbEntities
        {
            get { return DbEntities; }
        }

        public IEnumerable<ListItemModel> SearchByZipCode(string zipCode, int maxResultsOnQuery)
        {
            return DbEntities.ZipCode.Where(e => e.Code.Contains(zipCode))
                .Select(e => new ListItemModel { Value = e.Code, Key = e.ZipCodeId.ToString() })
                .OrderBy(e => e.Value)
                .Take(maxResultsOnQuery)
                .ToList();    
        }

        public int? SaveAddress(AddressInfoModel model, bool bIsNew)
        {
            var address = bIsNew ? new Entities.Address() : DbEntities.Address.Single(e => e.AddressId == model.AddressId);

            var phoneToAdd = new ClientPhone { ClientPhoneId = model.PrimaryPhone.PhoneId };
            DbEntities.ClientPhone.Attach(phoneToAdd);

            for (var i = address.ClientPhone.Count - 1; i >= 0; i--)
            {
                var phone = address.ClientPhone.ElementAt(i);
                address.ClientPhone.Remove(phone);
            }

            address.ClientPhone.Add(phoneToAdd);

            if (model.Country != null && model.Country.IdKey != null) address.CountryId = model.Country.IdKey.Value;
            if (model.RegionA != null && model.RegionA.IdKey != null) address.RegionArId = model.RegionA.IdKey.Value;
            if (model.RegionB != null && model.RegionB.IdKey != null) address.RegionBrId = model.RegionB.IdKey.Value;
            if (model.RegionC != null && model.RegionC.IdKey != null) address.RegionCrId = model.RegionC.IdKey.Value;
            if (model.RegionD != null && model.RegionD.IdKey != null) address.RegionDrId = model.RegionD.IdKey.Value;

            address.ExtIntNumber = model.ExtIntNumber;
            address.MainAddress = model.MainAddress;
            address.Reference = model.Reference;
            if (model.ZipCode.IdKey != null) address.ZipCodeId = model.ZipCode.IdKey.Value;

            if (bIsNew)
                DbEntities.Address.Add(address);
            DbEntities.SaveChanges();

            return address.AddressId;
        }

        public IEnumerable<AddressInfoModel> SearchAddressByPhoneId(int phoneId)
        {
            return DbEntities.ClientPhone.Where(e => e.ClientPhoneId == phoneId)
                .SelectMany(e => e.Address.Select(i => new
                {
                    i.AddressId,
                    e.ClientPhoneId,
                    e.Phone,
                    i.ClientPhone,
                    i.CountryId,
                    CountryName = i.Country.Name,
                    i.ExtIntNumber,
                    i.MainAddress,
                    i.Reference,
                    i.RegionArId,
                    RegionArName = i.RegionA.Name,
                    i.RegionBrId,
                    RegionBrName = i.RegionB.Name,
                    i.RegionCrId,
                    RegionCrName = i.RegionC.Name,
                    i.RegionDrId,
                    RegionDrName = i.RegionD.Name,
                    i.ZipCodeId,
                    i.ZipCode.Code
                }))
                .Select(e => new AddressInfoModel
            {
                AddressId = e.AddressId,
                Country = new ListItemModel { IdKey = e.CountryId, Value = e.CountryName},
                ExtIntNumber = e.ExtIntNumber,
                MainAddress = e.MainAddress,
                PrimaryPhone = new PhoneModel { Phone = e.Phone, PhoneId = e.ClientPhoneId },
                Reference = e.Reference,
                RegionA = new ListItemModel { IdKey = e.RegionArId, Value = e.RegionArName },
                RegionB = new ListItemModel { IdKey = e.RegionBrId, Value = e.RegionBrName },
                RegionC = new ListItemModel { IdKey = e.RegionCrId, Value = e.RegionCrName },
                RegionD = new ListItemModel { IdKey = e.RegionDrId, Value = e.RegionDrName },
                ZipCode = new ListItemModel { IdKey = e.ZipCodeId, Value = e.Code },
            }).ToList();
        }
    }
}
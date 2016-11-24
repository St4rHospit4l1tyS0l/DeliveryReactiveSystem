using System.Collections.Generic;
using System.Linq;
using Drs.Model.Address;
using Drs.Model.Order;
using Drs.Model.Shared;
using Drs.Model.Store;
using Drs.Repository.Entities;
using Drs.Repository.Shared;

namespace Drs.Repository.Address
{
    public class AddressRepository : BaseOneRepository, IAddressRepository
    {
        public AddressRepository()
        {
        }

        public AddressRepository(CallCenterEntities callCenter)
            : base(callCenter)
        {
        }

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

        public long SaveAddress(AddressInfoModel model, bool bIsNew)
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

            if (model.Country != null && model.Country.IdKey != null) address.CountryId = (int)model.Country.IdKey.Value;
            if (model.RegionA != null && model.RegionA.IdKey != null) address.RegionArId = (int)model.RegionA.IdKey.Value;
            if (model.RegionB != null && model.RegionB.IdKey != null) address.RegionBrId = (int)model.RegionB.IdKey.Value;
            if (model.RegionC != null && model.RegionC.IdKey != null) address.RegionCrId = (int)model.RegionC.IdKey.Value;
            if (model.RegionD != null && model.RegionD.IdKey != null) address.RegionDrId = (int)model.RegionD.IdKey.Value;

            address.ExtIntNumber = model.ExtIntNumber;
            address.MainAddress = model.MainAddress;
            address.Reference = model.Reference;
            if (model.ZipCode.IdKey != null) address.ZipCodeId = (int)model.ZipCode.IdKey.Value;
            address.IsMap = false;

            if (bIsNew)
                DbEntities.Address.Add(address);
            DbEntities.SaveChanges();

            return address.AddressId;
        }


        public long SaveAddressMap(AddressInfoModel model, bool bIsNew)
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

            address.CountryName = model.Country.Value;
            address.RegionNameA = model.RegionA.Value;
            address.RegionNameB = model.RegionB.Value;
            address.RegionNameC = model.RegionC.Value;
            address.RegionNameD = model.RegionD.Value;
            address.ZipCodeValue = model.ZipCode.Value;

            address.PlaceId = model.PlaceId;
            address.Lat = model.Lat;
            address.Lng = model.Lng;

            address.ExtIntNumber = model.ExtIntNumber;
            address.MainAddress = model.MainAddress;
            address.Reference = model.Reference;
            address.IsMap = true;

            if (bIsNew)
                DbEntities.Address.Add(address);
            DbEntities.SaveChanges();

            return address.AddressId;
        }

        public IEnumerable<AddressInfoModel> SearchAddressByPhoneId(long phoneId)
        {
            return DbEntities.ClientPhone.Where(e => e.ClientPhoneId == phoneId)
                .SelectMany(e => e.Address.Select(i => new
                {
                    i.AddressId,
                    e.ClientPhoneId,
                    e.Phone,
                    i.ClientPhone,
                    i.CountryId,
                    CountryName = i.IsMap ? i.CountryName : i.Country.Name,
                    i.ExtIntNumber,
                    i.MainAddress,
                    i.Reference,
                    i.RegionArId,
                    RegionArName = i.IsMap ? i.RegionNameA : i.RegionA.Name,
                    i.RegionBrId,
                    RegionBrName = i.IsMap ? i.RegionNameB : i.RegionB.Name,
                    i.RegionCrId,
                    RegionCrName = i.IsMap ? i.RegionNameC : i.RegionC.Name,
                    i.RegionDrId,
                    RegionDrName = i.IsMap ? i.RegionNameD : i.RegionD.Name,
                    i.ZipCodeId,
                    Code = i.IsMap ? i.ZipCodeValue : i.ZipCode.Code,
                    i.IsMap,
                    i.Lat,
                    i.Lng,
                    i.PlaceId
                }))
                .Select(e => new AddressInfoModel
                    {
                        AddressId = e.AddressId,
                        Country = new ListItemModel { IdKey = e.CountryId, Value = e.CountryName },
                        ExtIntNumber = e.ExtIntNumber,
                        MainAddress = e.MainAddress,
                        PrimaryPhone = new PhoneModel { Phone = e.Phone, PhoneId = e.ClientPhoneId },
                        Reference = e.Reference,
                        RegionA = new ListItemModel { IdKey = e.RegionArId, Value = e.RegionArName },
                        RegionB = new ListItemModel { IdKey = e.RegionBrId, Value = e.RegionBrName },
                        RegionC = new ListItemModel { IdKey = e.RegionCrId, Value = e.RegionCrName },
                        RegionD = new ListItemModel { IdKey = e.RegionDrId, Value = e.RegionDrName },
                        ZipCode = new ListItemModel { IdKey = e.ZipCodeId, Value = e.Code },
                        IsMap = e.IsMap,
                        Lat = e.Lat,
                        Lng = e.Lng,
                        PlaceId = e.PlaceId,
                    }
            ).ToList();
        }

        public long Add(AddressModel model)
        {
            var address = new Entities.Address
            {
                MainAddress = model.MainAddress,
                ExtIntNumber = model.NumExt,
                Reference = model.Reference,
                RegionArId = model.RegionArId,
                RegionBrId = model.RegionBrId,
                RegionCrId = model.RegionCrId,
                RegionDrId = model.RegionDrId,
                ZipCodeId = model.ZipCodeId
            };
            if (model.CountryId != null) address.CountryId = model.CountryId.Value;

            DbEntities.Address.Add(address);
            DbEntities.SaveChanges();
            return address.AddressId;
        }

        public void Update(long addressId, AddressModel model)
        {
            var address = DbEntities.Address.Single(e => e.AddressId == addressId);
            address.MainAddress = model.MainAddress;
            address.ExtIntNumber = model.NumExt;
            address.Reference = model.Reference;
            if (model.CountryId != null) address.CountryId = model.CountryId.Value;
            address.RegionArId = model.RegionArId;
            address.RegionBrId = model.RegionBrId;
            address.RegionCrId = model.RegionCrId;
            address.RegionDrId = model.RegionDrId;
            address.ZipCodeId = model.ZipCodeId;
            DbEntities.SaveChanges();
        }
    }
}
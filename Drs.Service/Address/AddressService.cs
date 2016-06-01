using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using Drs.Model.Address;
using Drs.Model.Constants;
using Drs.Model.Settings;
using Drs.Model.Shared;
using Drs.Repository.Address;
using Drs.Repository.Client;
using Drs.Service.Factory;

namespace Drs.Service.Address
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _repository;
        private readonly IClientRepository _repositoryClient;

        public AddressService(IAddressRepository repository, IClientRepository repositoryClient)
        {
            _repository = repository;
            _repositoryClient = repositoryClient;
        }

        public IEnumerable<AddressResponseSearch> SearchHierarchyByZipCode(string zipCode)
        {
            using (_repository)
            {
                var query = FactoryAddress.GetQueryToExecByZipCode(_repository.InnerDbEntities, zipCode);
                return query.ToList();
            }
        }

        public IEnumerable<ListItemModel> SearchByZipCode(string zipCode)
        {
            using (_repository)
            {
                return _repository.SearchByZipCode(zipCode, SettingsData.Server.MaxResultsOnQuery);
            }
        }

        public IEnumerable<ListItemModel> FillNextListByName(string sNextList, int iIdSelected, out string sControlName)
        {
            if (String.IsNullOrWhiteSpace(sNextList))
            {
                sNextList = SettingsData.FirstRegion;
                //foreach (var control in SettingsData.Constants.AddressUpsertSetting.LstAddressControls.Where(control => control.IsEnabled))
                //{
                //    sNextList = control.Name;
                //    break;
                //}
            }
            sControlName = sNextList;

            using (_repository)
            {
                var query = FactoryAddress.GetQueryToFillNextListByName(_repository.InnerDbEntities, sNextList, iIdSelected);
                return query.ToList();
            }
        }

        public ResponseMessageData<AddressInfoModel> SaveAddress(AddressInfoModel model)
        {
            using (_repository)
            {
                if (model.IsMap)
                {
                    model.AddressId = _repository.SaveAddressMap(model, model.AddressId == null || model.AddressId == SharedConstants.NULL_ID_VALUE);
                }
                else
                {
                    if (model.ZipCode == null || model.ZipCode.IdKey == null || model.ZipCode.IdKey <= 0)
                    {
                        var lastRegion = SettingsData.LastRegion;
                        model.ZipCode = FactoryAddress.GetQueryToGetByZipCodeId(_repository.InnerDbEntities, lastRegion, model);
                    }

                    model.AddressId = _repository.SaveAddress(model, model.AddressId == null || model.AddressId == SharedConstants.NULL_ID_VALUE);

                }

                return new ResponseMessageData<AddressInfoModel>
                {
                    Data = model,
                    IsSuccess = true,
                    Message = String.Empty
                };
            }
        }

        public IEnumerable<AddressInfoModel> SearchAddressByPhone(string phone)
        {
            if (String.IsNullOrWhiteSpace(phone))
                return new List<AddressInfoModel>();

            phone = phone.Trim().ToUpper();
            using (_repository)
            {
                int phoneId;
                using (_repositoryClient)
                {
                    phoneId = _repositoryClient.GetPhoneIdByPhone(phone);

                    if (phoneId == SharedConstants.NULL_ID_VALUE)
                        return new List<AddressInfoModel>();                    
                }

                return _repository.SearchAddressByPhoneId(phoneId);
            }   
        }
    }
}

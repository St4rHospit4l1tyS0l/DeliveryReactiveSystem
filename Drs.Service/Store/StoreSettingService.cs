using System;
using Drs.Infrastructure.Resources;
using Drs.Model.Constants;
using Drs.Model.Store;
using Drs.Repository.Address;
using Drs.Repository.Entities;
using Drs.Repository.Shared;
using Drs.Repository.Store;
using Drs.Service.Factory;

namespace Drs.Service.Store
{
    public class StoreSettingService : IDisposable
    {
        private readonly IStoreRepository _repositoryStore;
        private readonly AddressRepository _repositoryAddress;

        public StoreSettingService()
            :this(new CallCenterEntities())
        {
            
        }

        public StoreSettingService(CallCenterEntities callCenter)
            : this(new StoreRepository(callCenter), new AddressRepository(callCenter))
        {
        }

        public StoreSettingService(IStoreRepository repositoryStore, AddressRepository repositoryAddress)
        {
            _repositoryStore = repositoryStore;
            _repositoryAddress = repositoryAddress;
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_repositoryStore != null)
                    _repositoryStore.Dispose();

                if(_repositoryAddress != null)
                    _repositoryAddress.Dispose();
            }
        }

        public ResponseMessageModel ValidateModel(StoreUpModel model)
        {
            var response = new ResponseMessageModel {HasError = true};

            if (_repositoryStore.IsFranchiseValidById(model.FranchiseId) == false)
            {
                response.Message = "La franquicia no es válida";
                return response;
            }

            if (_repositoryStore.IsValidManagerStoreUserId(model.ManUserId) == false)
            {
                response.Message = "El usuario no tiene el perfil necesario, no es válido o no existe";
                return response;
            }

            var address = model.Address;
            if (address == null)
            {
                response.Message = "No se ha definido la dirección";
                return response;
            }


            var regionChild = FactoryAddress.GetRegionChildByZipCode();
            AddressModel addressRes = null;

            switch (regionChild)
            {
                case AddressConstants.REGION_A:
                    {
                        addressRes = _repositoryStore.IsValidRegionA(address.RegionArId);
                        break;
                    }
                case AddressConstants.REGION_B:
                    {
                        addressRes = _repositoryStore.IsValidRegionB(address.RegionBrId);
                        break;
                    }
                case AddressConstants.REGION_C:
                    {
                        addressRes = _repositoryStore.IsValidRegionC(address.RegionCrId);
                        break;
                    }
                case AddressConstants.REGION_D:
                    {
                        addressRes = _repositoryStore.IsValidRegionD(address.RegionDrId);
                        break;
                    }
            }

            if (addressRes == null)
            {
                response.Message = "La dirección no es correcta debido a la configuración";
                return response;
            }

            addressRes.MainAddress = address.MainAddress;
            addressRes.NumExt = address.NumExt;
            addressRes.Reference = address.Reference;

            model.AddressRes = addressRes;
            response.HasError = false;
            return response;
        }

        public ResponseMessageModel Save(StoreUpModel model)
        {
            var response = new ResponseMessageModel();

            if (model.FranchiseStoreId <= EntityConstants.NO_VALUE)
            {
                SaveAddress(model);
                _repositoryStore.Add(model);
            }
            else
            {
                SaveAddress(model);
                _repositoryStore.Update(model);
            }

            return response;
        }

        private void SaveAddress(StoreUpModel model)
        {
            if (model.AddressId <= EntityConstants.NO_VALUE)
            {
                model.AddressId = _repositoryAddress.Add(model.AddressRes);
            }
            else
            {
                _repositoryAddress.Update(model.AddressId, model.AddressRes);
            }
        }

        public void DoObsoleteStore(int id, string userId, ResponseMessageModel response)
        {
            var store = _repositoryStore.FindById(id);

            if (store == null)
            {
                response.HasError = true;
                response.Message = "El registro ya fue eliminado o no se encuentra";
                return;
            }

            _repositoryStore.DoObsoleteStore(store, userId);
        }
    }
}

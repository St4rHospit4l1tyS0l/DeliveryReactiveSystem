using System;
using Autofac;
using Drs.Model.Address;
using Drs.Model.Constants;
using Drs.Model.Properties;
using Drs.Model.Shared;
using Drs.Repository.Log;
using Drs.Service.Address;
using Drs.Service.Order;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ConnectCallCenter.Hubs
{
    [HubName(SharedConstants.Server.ADDRESS_HUB), UsedImplicitly]
    public class AddressHub : Hub
    {
        [HubMethodName(SharedConstants.Server.SEARCH_HIERARCHY_BY_ZIPCODE_ADDRESS_HUB_METHOD), UsedImplicitly]
        public ResponseMessageData<AddressResponseSearch> SearchHierarchyByZipCode(String zipCode)
        {
            try
            {
                return new ResponseMessageData<AddressResponseSearch>
                {
                    IsSuccess = true,
                    LstData = AppInit.Container.Resolve<IAddressService>().SearchHierarchyByZipCode(zipCode)
                };
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                return ResponseMessageData<AddressResponseSearch>.CreateCriticalMessage("No fue posible determinar la jerarquía con el código postal");
            }
        }

        [HubMethodName(SharedConstants.Server.SEARCH_BY_ZIPCODE_ADDRESS_HUB_METHOD), UsedImplicitly]
        public ResponseMessageData<ListItemModel> SearchByZipCode(String zipCode)
        {
            try
            {
                return new ResponseMessageData<ListItemModel>
                {
                    IsSuccess = true,
                    LstData = AppInit.Container.Resolve<IAddressService>().SearchByZipCode(zipCode)
                };
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                return ResponseMessageData<ListItemModel>.CreateCriticalMessage("No fue posible determinar la dirección con el código postal");

            }
        }
        
        [HubMethodName(SharedConstants.Server.FILL_NEXT_LIST_BYNAME_ADDRESS_HUB_METHOD), UsedImplicitly]
        public ResponseMessageData<ListItemModel> FillNextListByName(AddressQuery addressQuery)
        {
            try
            {
                string sControlName;
                var response = new ResponseMessageData<ListItemModel>
                {
                    IsSuccess = true,
                    LstData = AppInit.Container.Resolve<IAddressService>().FillNextListByName(addressQuery.NextRegion, addressQuery.ItemSelId, out sControlName),
                    Message = sControlName
                };

                return response;
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                return ResponseMessageData<ListItemModel>.CreateCriticalMessage("No fue posible determinar la siguiente región");
            }
        }


        [HubMethodName(SharedConstants.Server.SAVE_ADDRESS_ADDRESS_HUB_METHOD), UsedImplicitly]
        public ResponseMessageData<AddressInfoModel> SaveClient(AddressInfoModel model)
        {
            try
            {
                return AppInit.Container.Resolve<IAddressService>().SaveAddress(model);
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                return ResponseMessageData<AddressInfoModel>.CreateCriticalMessage("No fue posible guardar el cliente");

            }
        }
        
        [HubMethodName(SharedConstants.Server.SEARCH_ADDRESS_BY_PHONE_ADDRESS_HUB_METHOD), UsedImplicitly]
        public ResponseMessageData<AddressInfoModel> SearchAddressByPhone(String phone)
        {
            try
            {
                return new ResponseMessageData<AddressInfoModel>
                {
                    IsSuccess = true,
                    LstData = AppInit.Container.Resolve<IAddressService>().SearchAddressByPhone(phone)
                };
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                return ResponseMessageData<AddressInfoModel>.CreateCriticalMessage("No fue posible buscar una dirección por el teléfono");
            }
        }


        [HubMethodName(SharedConstants.Server.REMOVE_REL_PHONECLIENT_ADDRESS_HUB_METHOD), UsedImplicitly]
        public ResponseMessageData<bool> RemoveRelPhoneAddress(AddressPhoneModel model)
        {
            try
            {
                return AppInit.Container.Resolve<IOrderService>().RemoveRelPhoneAddress(model);
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                return ResponseMessageData<bool>.CreateCriticalMessage("No fue posible eliminar la relación teléfono-dirección");

            }
        }
    }

}
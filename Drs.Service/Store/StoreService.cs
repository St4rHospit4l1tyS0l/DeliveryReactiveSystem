using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Drs.Infrastructure.Extensions;
using Drs.Infrastructure.Extensions.Classes;
using Drs.Infrastructure.Extensions.Json;
using Drs.Infrastructure.Model;
using Drs.Model.Address;
using Drs.Model.Constants;
using Drs.Model.Franchise;
using Drs.Model.Order;
using Drs.Model.Settings;
using Drs.Model.Shared;
using Drs.Model.Store;
using Drs.Repository.Account;
using Drs.Repository.Client;
using Drs.Repository.Entities;
using Drs.Repository.Log;
using Drs.Repository.Shared;
using Drs.Repository.Store;
using Drs.Service.Account;
using Drs.Service.CustomerOrder;
using Drs.Service.Factory;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json.Linq;
using QueryFunctionClient = Drs.Service.QueryFunction.QueryFunctionClient;
using ResponseMessage = Drs.Model.Shared.ResponseMessage;

namespace Drs.Service.Store
{
    public class StoreService : IStoreService
    {
        private readonly IStoreRepository _repositoryStore;
        private readonly IClientRepository _repositoryClient;

        public StoreService(IStoreRepository repositoryStore, IClientRepository repositoryClient)
        {
            _repositoryStore = repositoryStore;
            _repositoryClient = repositoryClient;
        }

        public ResponseMessageData<OrderModelDto> SendOrderToStore(OrderModelDto model, IHubCallerConnectionContext<dynamic> clients)
        {
            var responseCod = new AccountService().IsValidInfoServer();
            var response = responseCod.DeserializeAndDecrypt<ConnectionInfoResponse>();
            var resMsg = new ResponseMessageData<OrderModelDto>();

            if (response.NxWn != SharedConstants.Client.STATUS_SCREEN_LOGIN)
            {
                resMsg.IsSuccess = false;
                resMsg.Message = response.Msg;
                return resMsg;
            }


            using (_repositoryStore)
            {
                var store = model.Store;

                //var store = FactoryAddress.GetQueryToSearchStore(_repositoryStore.InnerDbEntities, model.FranchiseCode, model.AddressInfo, out franchiseId);

                //TODO Falta vewrificar si tiene la capacidad para albergar una orden más

                if (store == null || store.IdKey.HasValue == false)
                {
                    resMsg.IsSuccess = false;
                    resMsg.Message = "No se encontró una tienda cercana a este domicilio, por favor reporte a soporte técnico";
                    return resMsg;
                }

                int franchiseId = _repositoryStore.GetFranchiseIdByStoreId(store.IdKey.Value);

                var offline = _repositoryStore.IsStoreOnline(store.IdKey.Value, DateTime.UtcNow);
                
                if(offline != null)
                {
                    resMsg.IsSuccess = false;
                    resMsg.Message = GetMessageStoreOffline(offline, store);
                    return resMsg;
                }

                model.Store = store;
                model.FranchiseId = franchiseId;
                model.UserId = AccountRepository.GetIdByUsername(model.Username, _repositoryStore.InnerDbEntities);
                _repositoryStore.SaveOrderToStore(model);
            }

            Task.Run(() => SendOrderToStoreEvent(model, clients));

            resMsg.Data = model;
            resMsg.IsSuccess = true;

            return resMsg;
        }

        private static string GetMessageStoreOffline(StoreOfflineDto offline, StoreModel store)
        {
            if (offline.IsUndefinedOfflineTime)
            {
                return String.Format("La tienda {0} no está en línea en estos momentos. Fuera de línea por tiempo indefinido.",
                    store.Value);
            }
            
            return String.Format("La tienda {0} no está en línea en estos momentos. Fuera de línea hasta {1}",
                store.Value, offline.DateTimeEnd.ToLocalTime().ToString(SharedConstants.DATE_TIME_FORMAT));
        }

        private void SendOrderToStoreEvent(OrderModelDto model, IHubCallerConnectionContext<dynamic> clients)
        {
            try
            {
                if (StoreIsAlive(model) == false)
                {
                    clients.Caller.OnSendToStoreEventChange(new ResponseMessage
                    {
                        Code = SettingsData.Constants.StoreConst.STORE_RESPONSE_PING_ERROR,
                        IsSuccess = false,
                        Message = String.Format("La tienda se encuentra fuera de línea, intente de nuevo por favor o reporte a su supervisor")
                    });

                    return;
                }

                clients.Caller.OnSendToStoreEventChange(new ResponseMessage
                {
                    Code = SettingsData.Constants.StoreConst.STORE_RESPONSE_PING_OK,
                    IsSuccess = true,
                    Message = String.Format("La tienda está en línea, se continua con el envío del pedido")
                });


                var order = GenerateCustomerOrder(model);
                var response = SendOrderToStore(model, clients, order);

                if (response == null)
                {
                    clients.Caller.OnSendToStoreEventChange(new ResponseMessage
                    {
                        Code = SettingsData.Constants.StoreConst.STORE_RESPONSE_FAILURE,
                        IsSuccess = true,
                        Message = String.Format("Después de varios intentos no fue posible enviar el envío a la tienda. Por favor intente de nuevo o reporte a su supervisor")
                    });
                    return;
                }


                clients.Caller.OnSendToStoreEventChange(new ResponseMessage
                {
                    Code = SettingsData.Constants.StoreConst.STORE_RESPONSE_ORDER_OK,
                    IsSuccess = true,
                    Message = String.Format("El pedido se ha enviado a la tienda de forma exitosa. Fecha y tiempo estimado de llegada {0:F}", 
                        response.Order.promiseTimeField.ToDateTimeSafe())
                });

                SaveRecurrence(model);
                SaveOrderStatus(model, response);

            }
            catch (Exception ex)
            {
                clients.Caller.OnSendToStoreEventChange(new ResponseMessage
                {
                    Code = SettingsData.Constants.StoreConst.STORE_RESPONSE_ORDER_ERROR,
                    IsSuccess = false,
                    Message = String.Format("Error al procesar el pedido: {0}", ex.Message)
                });
            }
        }

        private void SaveRecurrence(OrderModelDto model)
        {
            try
            {
                using (var repository = new StoreRepository())
                {
                    var now = DateTime.Now;
                    repository.SaveRecurrence(new Recurrence
                    {
                        ClientId = model.ClientId ?? EntityConstants.NULL_VALUE,
                        OrderToStoreId = model.OrderToStoreId,
                        Timestamp = now,
                        Total = (decimal)model.PosOrder.Total,
                        TimestampShort = now.ToDateShort()
                    });
                }

            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
            }
        }

        private void SaveOrderStatus(OrderModelDto model, ResponseRd response)
        {
            try
            {
                using (var repository = new StoreRepository())
                {
                    repository.UpdateOrderMode(model.OrderToStoreId, response.Order.orderIdField, response.Order.statusField,
                        response.Order.modeField, response.Order.modeChargeField, response.Order.promiseTimeField.ToDateTimeSafe());
                }

            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
            }
        }

        private bool StoreIsAlive(OrderModelDto model)
        {
            var iTries = 2;
            using (var client = new CustomerOrderClient(new BasicHttpBinding(), new EndpointAddress(model.Store.WsAddress + SettingsData.Constants.StoreOrder.WsCustomerOrder)))
            {
                while (iTries > 0)
                {
                    try
                    {
                        var result = client.Ping();
                        if (result == SettingsData.Constants.StoreConst.STORE_RESPONSE_PING_WS_OK)
                            return true;
                        iTries--;
                    }
                    catch (Exception)
                    {
                        iTries--;
                    }
                }

                client.Close();
            }
            return false;
        }

        private static ResponseRd SendOrderToStore(OrderModelDto model, IHubCallerConnectionContext<dynamic> clients, CustomerOrder.Order order)
        {
            using (var client = new CustomerOrderClient(new BasicHttpBinding(), new EndpointAddress(model.Store.WsAddress + SettingsData.Constants.StoreOrder.WsCustomerOrder)))
            {
                var iTries = 0;
                while (iTries < 3)
                {
                    try
                    {
                        var result = client.AddOrder(order);
                        if (result.IsSuccess)
                        {
                            clients.Caller.OnSendToStoreEventChange(new ResponseMessage
                            {
                                Code = SettingsData.Constants.StoreConst.STORE_RESPONSE_CALL_WS_SUCCESS,
                                IsSuccess = true,
                                Message = String.Format("Pedido enviado de forma exitosa")
                            });
                            client.Close();
                            return result;
                        }
                    }
                    catch (Exception ex)
                    {
                        SharedLogger.LogError(ex);
                        clients.Caller.OnSendToStoreEventChange(new ResponseMessage
                        {
                            Code = SettingsData.Constants.StoreConst.STORE_RESPONSE_CALL_WS_ERROR,
                            IsSuccess = false,
                            Message = String.Format("Intento {0} fallido. Error: {1}", (iTries + 1), ex.Message)
                        });
                    }
                    Thread.Sleep(1000);
                    iTries++;
                }

                client.Close();
            }

            return null;
        }

        private CustomerOrder.Order GenerateCustomerOrder(OrderModelDto model)
        {
            model.ClientInfo = _repositoryClient.GetClientById(model.ClientId ?? SettingsData.Constants.Entities.NULL_ID_INT);
            model.Phone = _repositoryClient.GetPhoneById(model.PhoneId);
            //var arrItem = new OrderItemsItem[model.PosOrder.LstItems.Count];
            var lstItem = new List<OrderItemsItem>();

            OrderItemsItem itemLevel0 = null;
            OrderItemsItemSubItemsItem itemLevel1 = null;
            var lstSubLevel0 = new List<OrderItemsItemSubItemsItem>();
            var lstSubLevel1 = new List<OrderItemsItemSubItemsItemSubItemsItem>();

            foreach (var item in model.PosOrder.LstItems)
            {
                switch (item.Level)
                {
                    case 2:
                        var itemL2 = new OrderItemsItemSubItemsItemSubItemsItem
                        {
                            menuItemIdField = item.ItemId.ToString(CultureInfo.InvariantCulture),
                            referenceIdField = item.CheckItemId.ToString(CultureInfo.InvariantCulture),
                            quantityField = SettingsData.Constants.StoreConst.QUANTITY_ITEM,
                            priceField = item.Price.ToString(CultureInfo.InvariantCulture),
                            levelField = item.Level.ToString(CultureInfo.InvariantCulture)
                        };

                        lstSubLevel1.Add(itemL2);

                        break;
                    case 1:

                        if (itemLevel1 != null && lstSubLevel1.Any())
                            itemLevel1.subItemsField = lstSubLevel1.ToArray();

                        var itemL1 = new OrderItemsItemSubItemsItem
                        {
                            menuItemIdField = item.ItemId.ToString(CultureInfo.InvariantCulture),
                            referenceIdField = item.CheckItemId.ToString(CultureInfo.InvariantCulture),
                            quantityField = SettingsData.Constants.StoreConst.QUANTITY_ITEM,
                            priceField = item.Price.ToString(CultureInfo.InvariantCulture),
                            levelField = item.Level.ToString(CultureInfo.InvariantCulture),
                        };

                        itemLevel1 = itemL1;
                        lstSubLevel0.Add(itemL1);
                        lstSubLevel1.Clear();

                        break;
                    default:

                        if (itemLevel0 != null && lstSubLevel0.Any())
                        {
                            if (lstSubLevel1.Any())
                                lstSubLevel0[lstSubLevel1.Count - 1].subItemsField = lstSubLevel1.ToArray();

                            itemLevel0.subItemsField = lstSubLevel0.ToArray();
                        }

                        itemLevel0 = new OrderItemsItem
                        {
                            menuItemIdField = item.ItemId.ToString(CultureInfo.InvariantCulture),
                            referenceIdField = item.CheckItemId.ToString(CultureInfo.InvariantCulture),
                            quantityField = SettingsData.Constants.StoreConst.QUANTITY_ITEM,
                            priceField = item.Price.ToString(CultureInfo.InvariantCulture),
                            levelField = item.Level.ToString(CultureInfo.InvariantCulture)
                        };

                        lstItem.Add(itemLevel0);
                        lstSubLevel0.Clear();
                        lstSubLevel1.Clear();
                        break;
                }
            }

            if (itemLevel0 != null && lstSubLevel0.Any())
            {
                if (lstSubLevel1.Any())
                    lstSubLevel0[lstSubLevel1.Count - 1].subItemsField = lstSubLevel1.ToArray();

                itemLevel0.subItemsField = lstSubLevel0.ToArray();
            }


            var order = new CustomerOrder.Order
            {
                customerField = new[]
                {
                    new OrderCustomer
                    {
                        firstNameField = model.ClientInfo.FirstName,
                        lastNameField = model.ClientInfo.LastName,
                        addressLine1Field = model.AddressInfo.MainAddress + 
                            (String.IsNullOrWhiteSpace(model.AddressInfo.ExtIntNumber) ? String.Empty : String.Format(" {0}", model.AddressInfo.ExtIntNumber)),
                        cityField = model.AddressInfo.RegionB.Value,
                        stateField = model.AddressInfo.RegionA.Value,
                        zipCodeField = model.AddressInfo.ZipCode.Value,
                        emailAddressField = model.ClientInfo.Email,
                        phoneNumberField = model.Phone,
                        notesField = model.Message,
                        addressNotesField = model.AddressInfo.Reference  
                    }
                },
                referenceIdField = model.OrderToStoreId.ToString(CultureInfo.InvariantCulture),
                modeField = model.OrderDetails.PosOrderMode,//SettingsData.Constants.StoreConst.SENDING_MODE_DELIVERY,
                //statusField = "InDelay",
                itemsField = new[]
                {
                    new OrderItems
                    {
                        itemField = lstItem.ToArray(),
                    }
                }
            };

            if (model.OrderDetails.PosOrderStatus == SettingsData.Constants.StoreConst.MODE_DELIVERY_FUTURE)
            {
                order.statusField = SettingsData.Constants.TrackConst.IN_DELAY;
                order.promiseTimeField = model.OrderDetails.PromiseTime.ToString("o");
            }

            order.orderNotesField = String.Empty;
            if (String.IsNullOrWhiteSpace(model.OrderDetails.ExtraNotes) == false)
            {
                order.orderNotesField = model.OrderDetails.ExtraNotes;
            }

            order.orderNotesField = String.Format("{0} | Método de pago: {1} |", order.orderNotesField, model.OrderDetails.Payment.Name);
            return order;
        }


        public ResponseMessage CancelOrder(long orderToStoreId)
        {
            using (_repositoryStore)
            {
                if (_repositoryStore.IsValidToCancel(orderToStoreId) == false)
                {
                    return new ResponseMessage
                    {
                        IsSuccess = false,
                        Message = "La orden no existe, está en un estado en la cual no puede ser cancelada o ya fue cancelada, por favor revise el historial"
                    };
                }

                var fsInfo = _repositoryStore.GetWsAddresInfoByOrderToStoreId(orderToStoreId);

                if (fsInfo == null)
                {
                    return new ResponseMessage
                    {
                        IsSuccess = false,
                        Message = "No existe configuración para realizar peticiones a la tienda"
                    };
                }

                if (String.IsNullOrWhiteSpace(fsInfo.AtoOrderId))
                {
                    return new ResponseMessage
                    {
                        IsSuccess = false,
                        Message = "El pedido nunca se envió a la tienda. No hay orden que cancelar."
                    };
                }

                long atoOrderId;

                if (long.TryParse(fsInfo.AtoOrderId, out atoOrderId) == false)
                {
                    return new ResponseMessage
                    {
                        IsSuccess = false,
                        Message = String.Format("El identificador del pedido ({0}) no es válido.", fsInfo.AtoOrderId)
                    };
                }

                //var respQuery = TrackService.GetOrderFromStore(fsInfo.AtoOrderId, fsInfo.WsAddress);
                //if (respQuery != null && respQuery.Order != null)
                //{
                //}

                var response = DoCancelOrder(atoOrderId, fsInfo);

                if (response.IsSuccess)
                {
                    try
                    {
                        _repositoryStore.SetCancelOrderToStore(orderToStoreId);
                    }
                    catch (Exception ex)
                    {
                        SharedLogger.LogError(ex);
                    } 
                }

                return response;
            }
        }


        public StoreModel StoreAvailableByStore(ItemCatalog item, ResponseMessageData<StoreModel> response)
        {
            using (_repositoryStore)
            {
                var store = _repositoryStore.GetStoreById(item.Id);
                return GetStoreAvailable(response, new List<StoreModel>{store}, false);
            }
        }

        public StoreModel StoreAvailableForAddressMap(StoreAvailableModel model, ResponseMessageData<StoreModel> response)
        {
            using (_repositoryStore)
            {
                var lstStoresCoverage = _repositoryStore.GetAvailableCoverageByFrachiseCode(model.FranchiseCode);

                if (lstStoresCoverage == null || lstStoresCoverage.Count == 0)
                {
                    response.IsSuccess = false;
                    response.Message = "La franquicia no tiene configurada la cobertura de sus tiendas";
                    return null;
                }

                var storesIds = CalculateStoresCoverages(lstStoresCoverage, model.AddressInfo);

                if (storesIds.Count == 0)
                {
                    response.IsSuccess = false;
                    response.Message = "No existe cobertura en esa dirección";
                    return null;
                }

                var stores = _repositoryStore.GetStoresByIds(storesIds);

                return GetStoreAvailable(response, stores, true);

            }
        }

        private List<int> CalculateStoresCoverages(List<CoverageStoreModel> storesCoverage, AddressInfoModel addressInfo)
        {
            var lstStoresIds = new List<int>();
            var gPoint = GeoHelper.PointFromText(addressInfo.Lat, addressInfo.Lng);

            foreach (var coverage in storesCoverage)
            {
                if (coverage.Coverage.Intersects(gPoint))
                {
                    if(lstStoresIds.Any(e => e == coverage.StoreId) == false)
                        lstStoresIds.Add(coverage.StoreId);
                }
            }
            
            return lstStoresIds;
        }


        public StoreModel StoreAvailableForAddress(StoreAvailableModel model, ResponseMessageData<StoreModel> response)
        {
            using (_repositoryStore)
            {
                int franchiseId;
                var stores = FactoryAddress.GetQueryToSearchStore(_repositoryStore.InnerDbEntities, model.FranchiseCode,
                    model.AddressInfo, out franchiseId);

                return GetStoreAvailable(response, stores, true);
            }
        }

        private StoreModel GetStoreAvailable(ResponseMessageData<StoreModel> response, List<StoreModel> stores, bool isByAddress)
        {
            if (stores.Any() == false)
            {
                response.IsSuccess = false;
                response.Message = "No hay una tienda disponible en la dirección que seleccionó";
                return null;
            }

            var store = stores[0];

            if (store.IdKey.HasValue == false)
            {
                response.IsSuccess = false;
                response.Message = "No hay una tienda disponible en la dirección que seleccionó";
                return null;
            }

            response.LstData = stores;

            if (isByAddress)
            {
                response.IsSuccess = true;
                response.Data = store;
                return store;
            }

            var utcDateTime = DateTime.UtcNow;

            var offline = _repositoryStore.IsStoreOnline(store.IdKey.Value, utcDateTime);
            if (offline == null)
            {
                response.IsSuccess = true;
                response.Data = store;
                return store;
            }

            response.IsSuccess = false;

            response.Message = GetMessageStoreOffline(offline, store);

            return null;
        }

        public void GetPreparationTime(string wsAddress, ResponseMessageData<StoreModel> response)
        {
            using (var client = new CustomerOrderClient(new BasicHttpBinding(), new EndpointAddress(wsAddress + SettingsData.Constants.StoreOrder.WsCustomerOrder)))
            {
                var iTries = 0;
                while (iTries < 3)
                {
                    try
                    {
                        var result = client.GetPreparationTime();
                        if (response.IsSuccess)
                        {
                            response.IsSuccess = true;
                            response.Message = String.Format("Entrega {0} mins", result.PrepTime);
                            return;
                        }

                        response.IsSuccess = false;
                        response.Message = result.ExcMsg;
                        return;
                    }
                    catch (Exception ex)
                    {
                        SharedLogger.LogError(ex);
                    }
                    Thread.Sleep(new Random().Next(50, 300));
                    iTries++;
                }

                client.Close();

                response.IsSuccess = false;
                response.Message = "No fue posible comunicarse a la tienda para obtener el tiempo de preparación ";
            }
        }


        private ResponseMessage DoCancelOrder(long atoOrderId, FranchiseStoreWsInfo fsInfo)
        {
            using (var client = new QueryFunctionClient(new BasicHttpBinding(), new EndpointAddress(fsInfo.WsAddress + SettingsData.Constants.StoreOrder.WsQueryFunction)))
            {
                var iTries = 0;
                while (iTries < 3)
                {
                    try
                    {
                        var result = client.CancelOrder(atoOrderId, false);
                        if (result.IsSuccess)
                        {
                            return new ResponseMessage
                            {
                                IsSuccess = true,
                                Message = "Orden cancelada de forma exitosa"
                            };
                        }

                        client.Close();
                        return new ResponseMessage
                        {
                            IsSuccess = false,
                            Message = "No es posible cancelar la orden debido a: " + result.ResultData
                        };
                    }
                    catch (Exception ex)
                    {
                        SharedLogger.LogError(ex);
                    }
                    Thread.Sleep(new Random().Next(200, 800));
                    iTries++;
                }

                client.Close();

                return new ResponseMessage
                {
                    IsSuccess = false,
                    Message = "No fue posible comunicarse a la tienda para realizar la cancelación. " +
                              "Por favor reporte a soporte técnico para revisar la conexión con la tienda " + fsInfo.Name
                };
            }
        }
    }
}

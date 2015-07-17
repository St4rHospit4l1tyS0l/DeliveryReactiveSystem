using System;
using System.Globalization;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Drs.Infrastructure.Extensions.Classes;
using Drs.Infrastructure.Extensions.Json;
using Drs.Infrastructure.Model;
using Drs.Model.Constants;
using Drs.Model.Franchise;
using Drs.Model.Order;
using Drs.Model.Settings;
using Drs.Model.Shared;
using Drs.Repository.Account;
using Drs.Repository.Client;
using Drs.Repository.Entities;
using Drs.Repository.Log;
using Drs.Repository.Shared;
using Drs.Repository.Store;
using Drs.Service.Account;
using Drs.Service.CustomerOrder;
using Drs.Service.Factory;
using Drs.Service.Track;
using Microsoft.AspNet.SignalR.Hubs;
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
            var responseCod = new AccountService().IsValidServerInfo();
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
                int franchiseId;
                var store = FactoryAddress.GetQueryToSearchStore(_repositoryStore.InnerDbEntities, model, out franchiseId);

                //TODO Falta vewrificar si tiene la capacidad para albergar una orden más

                if (store == null)
                {
                    resMsg.IsSuccess = false;
                    resMsg.Message = "No se encontró una tienda cercana a este domicilio, por favor reporte a soporte técnico";
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

                String promiseTime;
                try
                {
                    promiseTime = response.Order.promiseTimeField;
                }
                catch (Exception)
                {
                    promiseTime = SharedConstants.NOT_APPLICABLE;
                }

                clients.Caller.OnSendToStoreEventChange(new ResponseMessage
                {
                    Code = SettingsData.Constants.StoreConst.STORE_RESPONSE_ORDER_OK,
                    IsSuccess = true,
                    Message = String.Format("El pedido se ha enviado a la tienda de forma exitosa. Fecha y tiempo estimado de llegada {0}", promiseTime)
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
                        response.Order.modeField, response.Order.modeChargeField, response.Order.promiseTimeField);
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
            var arrItem = new OrderItemsItem[model.PosOrder.LstItems.Count];

            for (var i = 0; i < model.PosOrder.LstItems.Count; i++)
            {
                var item = model.PosOrder.LstItems[i];
                arrItem[i] = new OrderItemsItem
                {
                    menuItemIdField = item.ItemId.ToString(CultureInfo.InvariantCulture),
                    quantityField = SettingsData.Constants.StoreConst.QUANTITY_ITEM,
                    priceField = item.Price.ToString(CultureInfo.InvariantCulture)
                };
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
                modeField = SettingsData.Constants.StoreConst.SENDING_MODE_DELIVERY,
                //statusField = "InDelay",
                itemsField = new[]
                {
                    new OrderItems
                    {
                        itemField = arrItem,
                    }
                }
            };

            if (model.OrderDetails.OrderMode == SettingsData.Constants.StoreConst.MODE_DELIVERY_FUTURE)
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

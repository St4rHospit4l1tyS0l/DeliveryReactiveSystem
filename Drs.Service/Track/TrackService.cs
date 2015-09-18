using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using Drs.Model.Order;
using Drs.Model.Settings;
using Drs.Model.Shared;
using Drs.Model.Track;
using Drs.Repository.Store;
using Drs.Repository.Track;
using Drs.Service.QueryFunction;

namespace Drs.Service.Track
{
    public class TrackService : ITrackService
    {
        private readonly ITrackRepository _trackRepository;

        public TrackService(ITrackRepository trackRepository)
        {
            _trackRepository = trackRepository;
        }

        public IList<TrackOrderDto> SearchByPhone(PagerDto<String> phone)
        {
            using (_trackRepository)
            {
                return _trackRepository.SearchByPhone(phone);
            }
        }

        public IEnumerable<TrackOrderDto> SearchByClientName(PagerDto<string> clientName)
        {
            using (_trackRepository)
            {
                return _trackRepository.SearchByClientName(clientName);
            }
        }

        public TrackOrderDto ShowDetailByOrderId(long orderId)
        {
            using (_trackRepository)
            {
                var orderDetail = _trackRepository.ShowDetailByOrderId(orderId);

                if (orderDetail == null)
                    return null;

                var lastLog = orderDetail.LstOrderLog.OrderByDescending(e => e.Id).FirstOrDefault();
                var wsAddress = orderDetail.WsAddress;
                orderDetail.WsAddress = String.Empty;

                var now = DateTime.Now;

                if (lastLog == null || (lastLog.Timestamp.AddSeconds(SettingsData.SecondsToAskForStatusOrder) < now && lastLog.Status != SettingsData.Constants.TrackConst.CLOSED))
                {
                    var response = GetOrderFromStore(orderDetail.OrderAtoId, wsAddress);
                    if (response != null && response.Order != null)
                    {
                        using (var storeRepository = new StoreRepository())
                        {
                            var newStatusLog = storeRepository.SaveLogOrderToStore(orderId, "Se consulta el histórico de la orden", response.Order.statusField, DateTime.Now, true);

                            orderDetail.LstOrderLog.Add(new ItemLogOrder
                            {
                                Id = newStatusLog.OrderToStoreLogId,
                                Status = newStatusLog.Status,
                                Timestamp = newStatusLog.Timestamp
                            });

                            orderDetail.LastStatus = newStatusLog.Status;
                        }
                    }
                    else
                    {
                        orderDetail.StoreErrMsg = "No hay conexión con la tienda para obtener el último estado";
                    }
                }

                return orderDetail;
            }
        }

        public static ResponseRd GetOrderFromStore(string orderAtoId, string wsAddress)
        {
            using (var client = new QueryFunctionClient(new BasicHttpBinding(), new EndpointAddress(wsAddress + SettingsData.Constants.StoreOrder.WsQueryFunction)))
            {
                var iTries = 0;
                while (iTries < 2)
                {
                    try
                    {
                        var result = client.GetOrderById(long.Parse(orderAtoId));
                        if (!result.IsSuccess) 
                            continue;
                        client.Close();
                        return result;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    Thread.Sleep(200);
                    iTries++;
                }
                client.Close();
            }
            return null;
        }
    }
}

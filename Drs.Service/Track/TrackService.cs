using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using Drs.Model.Settings;
using Drs.Model.Shared;
using Drs.Model.Track;
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

        public IEnumerable<TrackOrderDto> SearchByPhone(PagerDto<String> phone)
        {
            using (_trackRepository)
            {
                return _trackRepository.SearchByPhone(phone);
            }
        }

        public IEnumerable<TrackOrderDto> SearchByClient(PagerDto<long> client)
        {
            using (_trackRepository)
            {
                return _trackRepository.SearchByClient(client);
            }
        }

        public TrackOrderDto ShowDetailByOrderId(long orderId)
        {
            using (_trackRepository)
            {
                var orderDetail = _trackRepository.ShowDetailByOrderId(orderId);

                if (orderDetail == null)
                    return null;

                orderDetail.WsAddress = String.Empty;

                return orderDetail;
            }
        }

        public IEnumerable<TrackOrderDto> SearchByDailyInfo(PagerDto<DailySearchModel> model)
        {
            using (_trackRepository)
            {
                return _trackRepository.SearchByDailyInfo(model);
            }
        }

        private static ResponseRd GetOrderFromStore(string orderAtoId, string wsAddress)
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

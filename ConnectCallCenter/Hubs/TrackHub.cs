using System;
using Autofac;
using Drs.Model.Constants;
using Drs.Model.Properties;
using Drs.Model.Shared;
using Drs.Model.Track;
using Drs.Repository.Log;
using Drs.Service.Track;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ConnectCallCenter.Hubs
{

    [HubName(SharedConstants.Server.TRACK_HUB), UsedImplicitly]
    public class TrackHub : Hub
    {
        [HubMethodName(SharedConstants.Server.SEARCH_BY_PHONE_TRACK_HUB_METHOD), UsedImplicitly]
        public ResponseMessageData<TrackOrderDto> SearchByPhone(PagerDto<String> phone)
        {
            try
            {
                var lstData = AppInit.Container.Resolve<ITrackService>().SearchByPhone(phone);
                return new ResponseMessageData<TrackOrderDto>
                {
                    IsSuccess = true,
                    LstData = lstData,
                    Pager = phone.Pager
                };
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                return ResponseMessageData<TrackOrderDto>.CreateCriticalMessage("No fue posible rastrear por número telefónico");
            }
        }

        [HubMethodName(SharedConstants.Server.SEARCH_BY_CLIENTNAME_TRACK_HUB_METHOD), UsedImplicitly]
        public ResponseMessageData<TrackOrderDto> SearchByClientName(PagerDto<int> client)
        {
            try
            {
                return new ResponseMessageData<TrackOrderDto>
                {
                    IsSuccess = true,
                    LstData = AppInit.Container.Resolve<ITrackService>().SearchByClient(client),
                    Pager = client.Pager
                };
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                return ResponseMessageData<TrackOrderDto>.CreateCriticalMessage("No fue posible rastrear por nombre del cliente");
            }
        }


        [HubMethodName(SharedConstants.Server.SEARCH_BY_DAILY_INFO_TRACK_HUB_METHOD), UsedImplicitly]
        public ResponseMessageData<TrackOrderDto> SearchByDailyInfo(PagerDto<DailySearchModel> model)
        {
            try
            {
                return new ResponseMessageData<TrackOrderDto>
                {
                    IsSuccess = true,
                    LstData = AppInit.Container.Resolve<ITrackService>().SearchByDailyInfo(model),
                    Pager = model.Pager
                };
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                return ResponseMessageData<TrackOrderDto>.CreateCriticalMessage("No fue posible obtener los pedidos con esos parámetros");
            }
        }

        [HubMethodName(SharedConstants.Server.SHOW_DETAIL_TRACK_HUB_METHOD), UsedImplicitly]
        public ResponseMessageData<TrackOrderDto> ShowDetailByOrderId(long orderId)
        {
            try
            {
                return new ResponseMessageData<TrackOrderDto>
                {
                    IsSuccess = true,
                    Data = AppInit.Container.Resolve<ITrackService>().ShowDetailByOrderId(orderId)
                };
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
                return ResponseMessageData<TrackOrderDto>.CreateCriticalMessage("No fue posible rastrear el detalle de la orden");
            }
        }

    }
}


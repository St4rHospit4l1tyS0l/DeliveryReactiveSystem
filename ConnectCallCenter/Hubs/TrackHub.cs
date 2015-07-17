using System;
using Autofac;
using Drs.Model.Constants;
using Drs.Model.Shared;
using Drs.Model.Track;
using Drs.Service.Track;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ConnectCallCenter.Hubs
{

    [HubName(SharedConstants.Server.TRACK_HUB)]
    public class TrackHub : Hub
    {
        [HubMethodName(SharedConstants.Server.SEARCH_BY_PHONE_TRACK_HUB_METHOD)]
        public ResponseMessageData<TrackOrderDto> SearchByPhone(String phone)
        {
            try
            {
                var lstData = AppInit.Container.Resolve<ITrackService>().SearchByPhone(phone);
                return new ResponseMessageData<TrackOrderDto>
                {
                    IsSuccess = true,
                    LstData = lstData
                };
            }
            catch (Exception ex)
            {
                return new ResponseMessageData<TrackOrderDto>
                {
                    IsSuccess = false,
                    Message = ex.Message + ex.StackTrace
                };
            }
        }

        [HubMethodName(SharedConstants.Server.SEARCH_BY_CLIENTNAME_TRACK_HUB_METHOD)]
        public ResponseMessageData<TrackOrderDto> SearchByClientName(String clientName)
        {
            try
            {
                return new ResponseMessageData<TrackOrderDto>
                {
                    IsSuccess = true,
                    LstData = AppInit.Container.Resolve<ITrackService>().SearchByClientName(clientName)
                };
            }
            catch (Exception ex)
            {
                return new ResponseMessageData<TrackOrderDto>
                {
                    IsSuccess = false,
                    Message = ex.Message + ex.StackTrace
                };
            }
        }

        [HubMethodName(SharedConstants.Server.SHOW_DETAIL_TRACK_HUB_METHOD)]
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
                return new ResponseMessageData<TrackOrderDto>
                {
                    IsSuccess = false,
                    Message = ex.Message + ex.StackTrace
                };
            }
        }

    }
}


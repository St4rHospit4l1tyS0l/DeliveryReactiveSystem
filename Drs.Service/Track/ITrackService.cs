using System;
using System.Collections.Generic;
using Drs.Model.Shared;
using Drs.Model.Track;

namespace Drs.Service.Track
{
    public interface ITrackService
    {
        IEnumerable<TrackOrderDto> SearchByPhone(PagerDto<String> phone);
        IEnumerable<TrackOrderDto> SearchByClient(PagerDto<long> client);
        TrackOrderDto ShowDetailByOrderId(long orderId);
        IEnumerable<TrackOrderDto> SearchByDailyInfo(PagerDto<DailySearchModel> model);
    }
}

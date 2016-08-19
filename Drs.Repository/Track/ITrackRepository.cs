using System;
using System.Collections.Generic;
using Drs.Model.Shared;
using Drs.Model.Track;

namespace Drs.Repository.Track
{
    public interface ITrackRepository : IDisposable
    {
        IEnumerable<TrackOrderDto> SearchByPhone(PagerDto<String> phone);
        IEnumerable<TrackOrderDto> SearchByClient(PagerDto<int> client);
        TrackOrderDetailDto ShowDetailByOrderId(long orderId);
        IEnumerable<TrackOrderDto> SearchByDailyInfo(PagerDto<DailySearchModel> model);
    }
}

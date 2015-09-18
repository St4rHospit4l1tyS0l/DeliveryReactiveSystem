using System;
using System.Collections.Generic;
using Drs.Model.Shared;
using Drs.Model.Track;

namespace Drs.Service.Track
{
    public interface ITrackService
    {
        IList<TrackOrderDto> SearchByPhone(PagerDto<String> phone);
        IEnumerable<TrackOrderDto> SearchByClientName(PagerDto<string> clientName);
        TrackOrderDto ShowDetailByOrderId(long orderId);
    }
}

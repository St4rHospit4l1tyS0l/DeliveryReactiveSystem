using System;
using System.Collections.Generic;
using Drs.Model.Shared;
using Drs.Model.Track;

namespace Drs.Repository.Track
{
    public interface ITrackRepository : IDisposable
    {
        IList<TrackOrderDto> SearchByPhone(PagerDto<String> phone);
        IList<TrackOrderDto> SearchByClientName(PagerDto<string> clientName);
        TrackOrderDetailDto ShowDetailByOrderId(long orderId);
    }
}

using System;
using System.Collections.Generic;
using Drs.Model.Track;

namespace Drs.Repository.Track
{
    public interface ITrackRepository : IDisposable
    {
        IList<TrackOrderDto> SearchByPhone(string phone);
        IList<TrackOrderDto> SearchByClientName(string clientName);
        TrackOrderDetailDto ShowDetailByOrderId(long orderId);
    }
}

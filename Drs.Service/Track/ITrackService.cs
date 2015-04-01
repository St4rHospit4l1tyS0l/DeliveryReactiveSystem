using System.Collections.Generic;
using Drs.Model.Track;

namespace Drs.Service.Track
{
    public interface ITrackService
    {
        IList<TrackOrderDto> SearchByPhone(string phone);
        IEnumerable<TrackOrderDto> SearchByClientName(string clientName);
        TrackOrderDto ShowDetailByOrderId(long orderId);
    }
}

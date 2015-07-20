using System.Collections.Generic;
using System.Linq;
using Drs.Model.Order;
using Drs.Model.Track;
using Drs.Repository.Shared;

namespace Drs.Repository.Track
{
    public class TrackRepository : BaseOneRepository, ITrackRepository
    {
        public IList<TrackOrderDto> SearchByPhone(string phone)
        {
            return DbEntities.OrderToStore.Where(e => e.OrderAtoId != null && e.ClientPhone.Phone.Contains(phone))
                .OrderByDescending(e => e.OrderToStoreId).Take(1500)
                .Select(e => new TrackOrderDto
                {
                    OrderToStoreId = e.OrderToStoreId,
                    Phone = e.ClientPhone.Phone,
                    StartDatetime = e.StartDatetime,
                    FirstName = e.Client.FirstName,
                    LastName = e.Client.LastName,
                    OrderAtoId = e.OrderAtoId,
                    StoreName = e.FranchiseStore.Name,
                    OrderTotal = e.PosOrder.Total,
                    LastStatus = e.LastStatus,
                    IsCanceled = e.IsCanceled
                }).ToList();
        }

        public IList<TrackOrderDto> SearchByClientName(string clientName)
        {
            return DbEntities.OrderToStore.Where(e => e.OrderAtoId != null &&
                (e.Client.FirstName.Contains(clientName) || e.Client.LastName.Contains(clientName) || e.Client.FirstName + " " + e.Client.LastName == clientName))
                .OrderByDescending(e => e.OrderToStoreId).Take(1500)
                .Select(e => new TrackOrderDto
                {
                    OrderToStoreId = e.OrderToStoreId,
                    Phone = e.ClientPhone.Phone,
                    StartDatetime = e.StartDatetime,
                    FirstName = e.Client.FirstName,
                    LastName = e.Client.LastName,
                    OrderAtoId = e.OrderAtoId,
                    StoreName = e.FranchiseStore.Name,
                    OrderTotal = e.PosOrder.Total,
                    LastStatus = e.LastStatus,
                    IsCanceled = e.IsCanceled
                }).ToList();
        }

        public TrackOrderDetailDto ShowDetailByOrderId(long orderId)
        {
            return DbEntities.OrderToStore
                .Select(e => new TrackOrderDetailDto
                {
                    OrderToStoreId = e.OrderToStoreId,
                    Phone = e.ClientPhone.Phone,
                    StartDatetime = e.StartDatetime,
                    EndDatetime = e.EndDatetime,
                    FirstName = e.Client.FirstName,
                    LastName = e.Client.LastName,
                    OrderAtoId = e.OrderAtoId,
                    StoreName = e.FranchiseStore.Name,
                    OrderTotal = e.PosOrder.Total,
                    LastStatus = e.LastStatus,
                    FranchiseName = e.Franchise.Name,
                    WsAddress = e.FranchiseStore.WsAddress,
                    Mode = e.OrderMode,
                    PromiseTime = e.PromiseTime,
                    UserTakeOrder = e.AspNetUsers.UserName,
                    LstOrderPos = e.PosOrder.PosOrderItem.Select(i => new ItemPosOrder{Name = i.Name, Price = i.Price, Level = i.LevelItem}).ToList(),
                    LstOrderLog = e.OrderToStoreLog.Select(i => new ItemLogOrder{Id = i.OrderToStoreLogId, Status = i.Status, Timestamp = i.Timestamp}).ToList()
                }).FirstOrDefault(e => e.OrderToStoreId == orderId);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Drs.Model.Order;
using Drs.Model.Settings;
using Drs.Model.Shared;
using Drs.Model.Track;
using Drs.Repository.Entities;
using Drs.Repository.Shared;

namespace Drs.Repository.Track
{
    public class TrackRepository : BaseOneRepository, ITrackRepository
    {
        public IEnumerable<TrackOrderDto> SearchByPhone(PagerDto<String> phone)
        {
            var query = DbEntities.OrderToStore.Where(e => e.OrderAtoId != null && e.ClientPhone.Phone.Contains(phone.Data))
                .Select(ExpGetTrackOrderDto());

            phone.Pager.Total = query.Count();
            return query.OrderByDescending(e => e.OrderToStoreId).Skip(phone.Pager.SkipRow).Take(phone.Pager.Size).ToList();
        }

        private static Expression<Func<OrderToStore, TrackOrderDto>> ExpGetTrackOrderDto()
        {
            return e => new TrackOrderDto
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
                IsCanceled = e.IsCanceled,
                MainAddress = e.Address.MainAddress + " | " + e.Address.ExtIntNumber,
                Agent = e.AspNetUsers.UserDetail.FirstName + " " + e.AspNetUsers.UserDetail.LastName + " (" + e.AspNetUsers.UserName + ")"
            };
        }

        public IEnumerable<TrackOrderDto> SearchByClient(PagerDto<int> client)
        {
            var clientId = client.Data;
            var query = DbEntities.OrderToStore.Where(e => e.OrderAtoId != null &&
                e.Client.ClientId == clientId)
                .Select(ExpGetTrackOrderDto());

            client.Pager.Total = query.Count();

            return query.OrderByDescending(e => e.OrderToStoreId).Skip(client.Pager.SkipRow).Take(client.Pager.Size).ToList();
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
                    ExtraNotes = e.ExtraNotes,
                    PromiseTime = e.PromiseTime,
                    UserTakeOrder = e.AspNetUsers.UserName,
                    LstOrderPos = e.PosOrder.PosOrderItem
                        .Select(i => new ItemPosOrder{Name = i.Name, Price = i.Price, Level = i.LevelItem, CheckItemId = i.CheckItemId})
                        .OrderBy(i => i.CheckItemId)
                        .ToList(),
                    LstOrderLog = e.OrderToStoreLog.Select(i => new ItemLogOrder{Id = i.OrderToStoreLogId, Status = i.Status, Timestamp = i.Timestamp}).ToList()
                }).FirstOrDefault(e => e.OrderToStoreId == orderId);
        }

        public IEnumerable<TrackOrderDto> SearchByDailyInfo(PagerDto<DailySearchModel> model)
        {
            var dailyInfo = model.Data;
            var query = DbEntities.OrderToStore.Where(e => e.OrderAtoId != null 
                && (
                    DbFunctions.TruncateTime(e.StartDatetime) == DbFunctions.TruncateTime(dailyInfo.SearchDate)
                    && (
                        dailyInfo.StoreId == SettingsData.Constants.Entities.NULL_ID_INT || e.FranchiseStoreId == dailyInfo.StoreId 
                    )
                    && (
                        dailyInfo.AgentId == String.Empty || e.UserInsId == dailyInfo.AgentId 
                    )
                ))
                .Select(ExpGetTrackOrderDto());

            model.Pager.ExtraData = query.Sum(e => (decimal?)e.OrderTotal) ?? 0;
            model.Pager.Total = query.Count();

            return query.OrderByDescending(e => e.OrderToStoreId).Skip(model.Pager.SkipRow).Take(model.Pager.Size).ToList();
        }
    }
}
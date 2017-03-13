using System;
using Drs.Infrastructure.Extensions;
using Drs.Model.Properties;

namespace Drs.Repository.Entities.Metadata
{
    public static class OrderInfoJson
    {
        [CanBeNull, UsedImplicitly]
        private static readonly ViewOrderInfo ModelEnt = null;

        public static readonly string Key = ModelEnt.PropertyName(e => e.OrderToStoreId);

        public static readonly string Columns = String.Join(",", new[]
        {
            ModelEnt.PropertyName(e => e.OrderToStoreId),
            ModelEnt.PropertyName(e => e.FranchiseName),
            ModelEnt.PropertyName(e => e.FranchiseStoreName),
            ModelEnt.PropertyName(e => e.StartDatetime),
            ModelEnt.PropertyName(e => e.LastStatus),
            ModelEnt.PropertyName(e => e.Phone),
            ModelEnt.PropertyName(e => e.FullName),
            ModelEnt.PropertyName(e => e.Address),
            ModelEnt.PropertyName(e => e.Total),
            ModelEnt.PropertyName(e => e.UserName)
        });

        public static OrderInfoDto DynamicToDto(dynamic data)
        {
            var model = new OrderInfoDto
            {
                OrderToStoreId = data.OrderToStoreId,
                FranchiseName = data.FranchiseName,
                FranchiseStoreName = data.FranchiseStoreName,
                StartDatetime = data.StartDatetime,
                LastStatus = data.LastStatus,
                Phone = data.Phone,
                FullName = data.FullName,
                Address = data.Address,
                Total = data.Total,
                UserName = data.UserName
            };

            return model;
        }
    }
}
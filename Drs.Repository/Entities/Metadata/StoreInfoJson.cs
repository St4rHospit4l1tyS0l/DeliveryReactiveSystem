using System;
using System.Data;
using Drs.Infrastructure.Extensions;
using Drs.Model.Annotations;

namespace Drs.Repository.Entities.Metadata
{
    public static class StoreInfoJson
    {
        [UsedImplicitly]
        private static ViewStoreInfo _modelEnt = null;
        public static readonly string Key = _modelEnt.PropertyName(e => e.FranchiseStoreId);

        public static readonly string Columns = String.Join(",", new[]
        {
            _modelEnt.PropertyName(e => e.FranchiseStoreId),
            _modelEnt.PropertyName(e => e.Name),
            _modelEnt.PropertyName(e => e.FranchiseName),
            _modelEnt.PropertyName(e => e.Address),
            _modelEnt.PropertyName(e => e.WsAddress),
            _modelEnt.PropertyName(e => e.UserNameIns),
            _modelEnt.PropertyName(e => e.IsObsolete)
        });

        public static StoreInfoDto DynamicToDto(dynamic data)
        {
            var model = new StoreInfoDto
            {
                FranchiseStoreId = data.FranchiseStoreId,
                Name = data.Name,
                FranchiseName = data.FranchiseName,
                Address = data.Address,
                WsAddress = data.WsAddress,
                UserNameIns = data.UserNameIns
            };

            return model;
        }
    }
}
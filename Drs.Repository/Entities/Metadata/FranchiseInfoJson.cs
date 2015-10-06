using System;
using Drs.Infrastructure.Extensions;
using Drs.Model.Annotations;

namespace Drs.Repository.Entities.Metadata
{
    public static class FranchiseInfoJson
    {
        [CanBeNull, UsedImplicitly]
        private static Franchise _modelEnt = null;

        public static readonly string Key = _modelEnt.PropertyName(e => e.FranchiseId);

        public static readonly string Columns = String.Join(",", new[]
        {
            _modelEnt.PropertyName(e => e.FranchiseId),
            _modelEnt.PropertyName(e => e.Name),
            _modelEnt.PropertyName(e => e.Code),
            _modelEnt.PropertyName(e => e.FranchiseButton.Position),
            _modelEnt.PropertyName(e => e.AspNetUsers.UserName),
        });

        public static FranchiseInfoDto DynamicToDto(dynamic data)
        {
            var model = new FranchiseInfoDto
            {
                FranchiseId = data.FranchiseId,
                Name = data.Name,
                Code = data.Code,
                Position = data.Position,
                UserNameIns = data.UserName,
            };

            return model;
        }
    }
}
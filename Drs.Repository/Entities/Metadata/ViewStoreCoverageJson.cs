using System;
using Drs.Infrastructure.Extensions;
using Drs.Model.Properties;

namespace Drs.Repository.Entities.Metadata
{
    public static class ViewStoreCoverageJson
    {
        [UsedImplicitly]
        private static ViewStoreCoverage _modelEnt = new ViewStoreCoverage();
        public static readonly string Key = _modelEnt.PropertyName(e => e.FranchiseId);

        public static readonly string Columns = String.Join(",", new[]
        {
            _modelEnt.PropertyName(e => e.FranchiseId),
            _modelEnt.PropertyName(e => e.Name),
            _modelEnt.PropertyName(e => e.CountStore),
            _modelEnt.PropertyName(e => e.IsObsolete)
        });

        public static ViewStoreCoverageDto DynamicToDto(dynamic data)
        {
            var model = new ViewStoreCoverageDto
            {
                FranchiseId = data.FranchiseId,
                Name = data.Name,
                CountStore = data.CountStore,
                IsObsolete = data.IsObsolete
            };

            return model;
        }
    }
}
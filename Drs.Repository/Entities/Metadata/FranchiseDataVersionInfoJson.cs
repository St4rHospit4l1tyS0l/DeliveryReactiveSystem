using System;
using Drs.Infrastructure.Extensions;
using Drs.Model.Annotations;

namespace Drs.Repository.Entities.Metadata
{
    public static class FranchiseDataVersionInfoJson
    {
        [CanBeNull, UsedImplicitly]
        private static FranchiseDataVersion _modelEnt = null;

        public static readonly string Key = _modelEnt.PropertyName(e => e.FranchiseDataVersionId);

        public static readonly string Columns = String.Join(",", new[]
        {
            _modelEnt.PropertyName(e => e.FranchiseDataVersionId),
            _modelEnt.PropertyName(e => e.Version),
            _modelEnt.PropertyName(e => e.Timestamp),
            _modelEnt.PropertyName(e => e.TotalNumberOfFiles),
            _modelEnt.PropertyName(e => e.NumberOfFilesDownloaded),
            _modelEnt.PropertyName(e => e.IsCompleted),
            _modelEnt.PropertyName(e => e.TimestampComplete),
            _modelEnt.PropertyName(e => e.AspNetUsers.UserName)
        });

        public static FranchiseDataVersionInfoDto DynamicToDto(dynamic data)
        {
            var model = new FranchiseDataVersionInfoDto
            {
                FranchiseDataVersionId = data.FranchiseDataVersionId,
                Version = data.Version,
                TimestampDt = data.Timestamp,
                TotalNumberOfFiles = data.TotalNumberOfFiles,
                NumberOfFilesDownloaded = data.NumberOfFilesDownloaded,
                IsCompletedBl = data.IsCompleted,
                TimestampCompleteDt = data.TimestampComplete,
                UserName = data.UserName,
            };

            return model;
        }
    }
}
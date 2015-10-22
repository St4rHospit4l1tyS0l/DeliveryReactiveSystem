using System;
using System.Globalization;
using System.ServiceModel;
using Drs.Infrastructure.Extensions;
using Drs.Infrastructure.Resources;
using Drs.Model.Franchise;
using Drs.Model.Settings;
using Drs.Repository.Entities;
using Drs.Repository.Order;
using Drs.Repository.Shared;
using Drs.Service.SyncFranchiseSvc;
using Drs.Service.Transport;

namespace Drs.Service.Franchise
{
    public class FranchiseSettingService : IDisposable
    {
        private readonly IFranchiseRepository _repository;

        public FranchiseSettingService()
        {
            _repository = new FranchiseRepository();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing) 
                return;
            
            if (_repository != null)
                _repository.Dispose();
        }

        public ResponseMessageModel ValidateModel(FranchiseUpModel model)
        {
            var response = new ResponseMessageModel { HasError = true };

            if (_repository.IsPositionAlreadyUsed(model.Position, model.FranchiseId))
            {
                response.Message = "La posición del botón ya ha sido usada por otra franquicia";
                return response;
            }

            model.Code = model.Code.Trim().ToUpper();

            if (model.Code.Contains(" "))
            {
                response.Message = "El código no puede contener espacios en blanco";
                return response;
            }

            if (_repository.IsCodeAlreadyUsed(model.Code, model.FranchiseId))
            {
                response.Message = "El código ya ha sido usada por otra franquicia";
                return response;
            }

            response.HasError = false;
            return response;
        }

        public ResponseMessageModel Save(FranchiseUpModel model)
        {
            var response = new ResponseMessageModel();

            if (model.FranchiseId <= EntityConstants.NO_VALUE)
            {
                _repository.Add(model);
            }
            else
            {
                _repository.Update(model);
            }

            return response;
        }

        public void DoObsolete(int id, string userId, ResponseMessageModel response)
        {
            var model = _repository.FindById(id);

            if (model == null)
            {
                response.HasError = true;
                response.Message = "El registro ya fue eliminado o no se encuentra";
                return;
            }

            _repository.DoObsolete(model, userId);

        }

        public void CreateNewVersion(int franchiseId, ResponseMessageModel response, string userId)
        {
            var now = DateTime.Now;
            var model = new FranchiseDataVersion
            {
                FranchiseId = franchiseId,
                FranchiseDataVersionUid = Guid.NewGuid(),
                Version = now.Ticks.ToString(CultureInfo.InvariantCulture).ToVersion(4, 3, '.'),
                Timestamp = now,
                NumberOfFilesDownloaded = 0,
                IsCompleted = false,
                IsListOfFilesReceived = false,
                TimestampComplete = null,
                UserInsId = userId,
                IsObsolete = false
            };

            var wsUrl = _repository.GetUrlSyncWsByFranchiseId(franchiseId);

            model.TotalNumberOfFiles = FranchiseQueryForFiles(model, response, wsUrl);

            if (response.HasError)
                return;

            _repository.SaveFranchiseDataVersion(model);
            response.HasError = false;
        }

        private int FranchiseQueryForFiles(FranchiseDataVersion model, ResponseMessageModel response, string wsUrl)
        {
            using (var client = new SyncFranchiseClient(new BasicHttpBinding(), new EndpointAddress(wsUrl + SettingsData.Constants.Franchise.WS_SYNC_FILES)))
            {
                WcfExt.SetMtomEncodingAndSize(client.Endpoint);

                var res = client.QueryForFiles(model.FranchiseDataVersionUid);

                if (res.HasError == false)
                {
                    response.HasError = false;
                    return res.TotalFiles;
                }

                response.Message = res.Message;
                response.HasError = true;
                return EntityConstants.NO_VALUE;
            }
        }

        public void DoObsoleteVersion(int id, string userId, ResponseMessageModel response)
        {
            _repository.DoObsoleteVersion(id, userId, response);
        }
    }
}

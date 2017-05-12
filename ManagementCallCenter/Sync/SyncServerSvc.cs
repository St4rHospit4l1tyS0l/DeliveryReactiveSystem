using System;
using System.IO;
using System.ServiceModel;
using Drs.Infrastructure.Crypto;
using Drs.Model.Settings;
using Drs.Model.Sync;
using Drs.Repository.Order;
using Drs.Repository.Shared;

namespace ManagementCallCenter.Sync
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SyncServerSvc" in both code and config file together.
    public class SyncServerSvc : ISyncServerSvc
    {
        public ResponseMessageServerFileSync GetFileByName(RequestMessageServerFileSync request)
        {
            switch (request.FileType)
            {
                case SettingsData.Constants.FranchiseConst.SYNC_FILE_TYPE_DATA:
                    return GetSyncDataFileStream(request);

                case SettingsData.Constants.FranchiseConst.SYNC_FILE_TYPE_LOGO:
                case SettingsData.Constants.FranchiseConst.SYNC_FILE_TYPE_IMAGE_NOTIFICATION:
                    return GetSyncResourceFileStream(request);

            }

            throw new NotSupportedException("El tipo de archivo no está soportado");
        }

        private static ResponseMessageServerFileSync GetSyncResourceFileStream(RequestMessageServerFileSync request)
        {
            var response = new ResponseMessageServerFileSync();
            try
            {
                string path;

                using (var repository = new ResourceRepository())
                {
                    path = repository.GetResourcePath(request.FileName);
                }

                if (String.IsNullOrWhiteSpace(path))
                {
                    response.HasError = true;
                    response.Message = String.Format("La ruta para el archivo {0} no se encuentra. Por favor revise de nuevo",
                        request.FileName);
                    return response;
                }


                var pathFileName = Path.Combine(path, request.FileName);

                if (File.Exists(pathFileName) == false)
                {
                    response.HasError = true;
                    response.Message = String.Format("El archivo {0} no se encuentra. Por favor revise de nuevo",
                        request.FileName);
                    return response;
                }

                response.File = File.OpenRead(pathFileName);


                var clientContext = OperationContext.Current;
                clientContext.OperationCompleted += delegate
                {
                    if (response.File != null)
                        response.File.Dispose();
                };

                response.HasError = false;
                return response;
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = ex.Message + " - " + ex.StackTrace;
                return response;
            }
        }

        private static ResponseMessageServerFileSync GetSyncDataFileStream(RequestMessageServerFileSync request)
        {
            var response = new ResponseMessageServerFileSync();
            try
            {
                var pathFileName = Path.Combine(SettingsData.Server.PathToSaveSyncFiles, request.UidVersion.ToString(),
                    request.FileName);

                if (File.Exists(pathFileName) == false)
                {
                    response.HasError = true;
                    response.Message = String.Format("El archivo {0} no se encuentra. Por favor revise de nuevo",
                        request.FileName);
                    return response;
                }

                response.File = File.OpenRead(pathFileName);


                var clientContext = OperationContext.Current;
                clientContext.OperationCompleted += delegate
                {
                    if (response.File != null)
                        response.File.Dispose();
                };

                response.HasError = false;
                return response;
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = ex.Message + " - " + ex.StackTrace;
                return response;
            }
        }

        public void SetFranchiseVersionTerminalOk(int franchiseId, string sVersion, string sMaInfo)
        {
            using (var repository = new FranchiseRepository())
            {
                repository.SetFranchiseVersionTerminalOk(franchiseId,  sVersion,  Cypher.Encrypt(sMaInfo));
            }
        }

    }
}

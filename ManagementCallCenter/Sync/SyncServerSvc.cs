using System;
using System.IO;
using System.ServiceModel;
using Drs.Infrastructure.Crypto;
using Drs.Model.Settings;
using Drs.Model.Sync;
using Drs.Repository.Order;

namespace ManagementCallCenter.Sync
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SyncServerSvc" in both code and config file together.
    public class SyncServerSvc : ISyncServerSvc
    {
        public ResponseMessageServerFileSync GetFileByName(RequestMessageServerFileSync request)
        {
            var response = new ResponseMessageServerFileSync();
            try
            {
                var pathFileName = Path.Combine(SettingsData.Server.PathToSaveSyncFiles, request.UidVersion.ToString(), request.FileName);

                if (File.Exists(pathFileName) == false)
                {
                    response.HasError = true;
                    response.Message = String.Format("El archivo {0} no se encuentra. Por favor revise de nuevo", request.FileName);
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

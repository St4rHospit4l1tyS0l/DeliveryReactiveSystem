using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using Drs.Infrastructure.Resources;
using Drs.Model.Address;
using Drs.Model.Franchise;
using Drs.Repository.Log;
using Drs.Repository.Order;
using Newtonsoft.Json.Linq;

namespace Drs.Service.Franchise
{
    public class FranchiseCoverageService
    {
        public ResponseMessageModel DoSave(FranchiseCoverageModel franchiseCoverage, string userId)
        {
            var response = new ResponseMessageModel {HasError = true, Title = "Guardar información"};

            if (string.IsNullOrWhiteSpace(franchiseCoverage.LastConfig))
            {
                response.Message = "Error en la red o en el navegador, por favor reinicie su navegador e intente de nuevo.";
                return response;
            }

            if (string.IsNullOrWhiteSpace(franchiseCoverage.Stores))
            {
                response.Message = "No existe información definida para generar las coberturas de la franquicia";
                return response;
            }

            var lstFranchiseCoverage = ExtractCoverages(franchiseCoverage, response);
            if (response.HasError)
                return response;

            using (var repository = new FranchiseRepository())
            {
                using (var transaction = repository.Db.Database.BeginTransaction())
                {
                    if (repository.AnyFranchiseById(franchiseCoverage.Id) == false)
                    {
                        response.Message = "No se ha encontrado la Franquicia o ésta ya fue eliminada";
                        return response;
                    }

                    var lastCoverage = repository.GetFranchiseCoverageById(franchiseCoverage.Id);

                    if(lastCoverage != null)
                        repository.BackupFranchiseCoverageById(lastCoverage);

                    repository.SaveFranchiseCoverage(franchiseCoverage, lastCoverage, userId, lstFranchiseCoverage);

                    transaction.Commit();
                }
            }

            response.Message = "La información fue almacenada de forma correcta";
            response.HasError = false;
            return response;
        }

        private List<SetCoverageStoreModel> ExtractCoverages(FranchiseCoverageModel franchiseCoverage, ResponseMessageModel response)
        {
            var lstPoints = new List<string>();
            
            try
            {
                dynamic coverages = JArray.Parse(franchiseCoverage.Stores);
                var lstCoverageStore = new  List<SetCoverageStoreModel>();

                foreach (var coverage in coverages)
                {
                    var store = new SetCoverageStoreModel { StoreId = coverage.id, Coverage = new List<DbGeography>() };
                    foreach (var path in coverage.paths)
                    {
                        lstPoints = new List<string>();
                        foreach (var point in path.path)
                        {
                            lstPoints.Add(String.Format("{0} {1}", point.lng, point.lat).Replace(",", "."));
                        }

                        if (lstPoints.Count > 0)
                        {
                            lstPoints.Add(lstPoints[0]);
                        }

                        lstPoints.Reverse();

                        store.Coverage.Add(GeoHelper.PolygonFromText(String.Format("({0})", String.Join(", ", lstPoints))));
                    }

                    lstCoverageStore.Add(store);
                }

                if (lstCoverageStore.Count == 0)
                {
                    response.HasError = true;
                    response.Message = "No existen coberturas válidas para esta franquicia";
                    return null;
                }

                response.HasError = false;
                return lstCoverageStore;
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = "No fue posible obtener las coberturas debido a: " + ex.Message;
                SharedLogger.LogError(ex, franchiseCoverage, String.Format("({0})", String.Join(", ", lstPoints)));
                return null;
            }
        }
    }
}

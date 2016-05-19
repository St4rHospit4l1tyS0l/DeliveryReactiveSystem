using Drs.Infrastructure.Resources;
using Drs.Model.Franchise;
using Drs.Repository.Order;

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

            using (var repository = new FranchiseRepository())
            {
                if (repository.AnyFranchiseById(franchiseCoverage.Id) == false)
                {
                    response.Message = "No se ha encontrado la Franquicia o ésta ya fue eliminada";
                    return response;
                }

                var lastCoverage = repository.GetFranchiseCoverageById(franchiseCoverage.Id);

                if(lastCoverage != null)
                    repository.BackupFranchiseCoverageById(lastCoverage);

                repository.SaveFranchiseCoverage(franchiseCoverage, lastCoverage, userId);
            }

            response.Message = "La información fue almacenada de forma correcta";
            response.HasError = false;
            return response;
        }
    }
}

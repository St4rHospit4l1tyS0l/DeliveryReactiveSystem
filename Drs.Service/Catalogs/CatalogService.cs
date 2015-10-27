using Drs.Model.Settings;
using Drs.Repository.Catalog;

namespace Drs.Service.Catalogs
{
    public class CatalogService : ICatalogService
    {
        private readonly ICatalogRepository _repository;
        public CatalogService(ICatalogRepository repository)
        {
            _repository = repository;
        }

        public ResponseCatalogs FindAllCatalogs()
        {
            using (_repository)
            {
                var lstPayments = _repository.GetPayments();
                var lstDeliveryStatus = _repository.GetDeliveryStatus();
                var lstStores = _repository.GetStores();

                return new ResponseCatalogs
                {
                    IsSuccess = true,
                    LstPayments = lstPayments,
                    LstDeliveryStatus = lstDeliveryStatus,
                    LstStores = lstStores
                };
            }
        }
    }
}

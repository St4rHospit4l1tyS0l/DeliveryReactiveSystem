using System;
using Drs.Model.Settings;
using Drs.Service.Catalogs;

namespace ManagementCallCenter.Catalogs
{

    public class CatalogsSvc : ICatalogsSvc
    {
        private readonly ICatalogService _service;

        public CatalogsSvc(ICatalogService service)
        {
            _service = service;
        }

        public ResponseCatalogs FindAllCatalogs()
        {
            try
            {
                return _service.FindAllCatalogs();
            }
            catch (Exception ex)
            {
                return new ResponseCatalogs { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}

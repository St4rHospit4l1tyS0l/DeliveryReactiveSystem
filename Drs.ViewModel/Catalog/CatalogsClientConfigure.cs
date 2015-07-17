using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Drs.Model.Catalog;
using Drs.Repository.Catalog;
using Drs.ViewModel.CatalogsSvc;
using ItemCatalog = Drs.Model.Shared.ItemCatalog;

namespace Drs.ViewModel.Catalog
{
    public static class CatalogsClientConfigure
    {
        public static async Task Initialize()
        {
            using (var catalogsSvc = new CatalogsSvcClient())
            {
                var catalogs = await catalogsSvc.FindAllCatalogsAsync();

                if (catalogs.IsSuccess == false)
                    throw new Exception("Se sucitó el siguiente error: " + catalogs.Message);

                CatalogsClientModel.CatPayments = FillCategory(catalogs.LstPayments);
                CatalogsClientModel.DicOrderStatus = FillDicCategory(catalogs.LstDeliveryStatus);

            }
        }

        public static void Initialize(ICatalogRepository repository)
        {
            CatalogsClientModel.CatPayments = FillCategory(repository.GetPayments());
            CatalogsClientModel.DicOrderStatus = FillDicCategory(repository.GetDeliveryStatus());
        }

        private static Dictionary<string, ItemCatalog> FillDicCategory(IEnumerable<ItemCatalog> lstCatalogs)
        {
            return lstCatalogs.ToDictionary(item => item.Code, item => new ItemCatalog
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                Value = item.Value
            });
        }

        private static List<ItemCatalog> FillCategory(IEnumerable<ItemCatalog> lstCatalog)
        {
            return lstCatalog.Select(item => new ItemCatalog { Id = item.Id, Name = item.Name }).ToList();
        }

        private static Dictionary<string, ItemCatalog> FillDicCategory(IEnumerable<CatalogsSvc.ItemCatalog> lstCatalogs)
        {
            return lstCatalogs.ToDictionary(item => item.Code, item => new ItemCatalog
            {
                Id = item.Id, Code = item.Code, Name = item.Name, Value = item.Value
            });
        }

        private static List<ItemCatalog> FillCategory(IEnumerable<CatalogsSvc.ItemCatalog> lstCatalog)
        {
            return lstCatalog.Select(item => new ItemCatalog {Id = item.Id, Name = item.Name}).ToList();
        }
    }
}

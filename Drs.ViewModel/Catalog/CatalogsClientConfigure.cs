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
                CatalogsClientModel.DicFranchiseStore = FillDicLstCategory(catalogs.LstStores);
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
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                Value = item.Value
            });
        }


        private static Dictionary<string, List<ItemCatalog>> FillDicLstCategory(IList<CatalogsSvc.ItemCatalog> lstCatalogs)
        {
            var dictLstItemCatalog = new Dictionary<string, List<ItemCatalog>>();

            foreach (var item in lstCatalogs)
            {
                List<ItemCatalog> lstCat;
                if (dictLstItemCatalog.TryGetValue(item.Code, out lstCat))
                {
                    lstCat.Add(new ItemCatalog
                    {
                        Id = item.Id,
                        Code = item.Code,
                        Name = String.Format("{0} ({1})", item.Name, item.Value),
                        Value = item.Value
                    });
                }
                else
                {
                    dictLstItemCatalog.Add(item.Code, new List<ItemCatalog>{new ItemCatalog
                    {
                        Id = item.Id,
                        Code = item.Code,
                        Name = String.Format("{0} ({1})", item.Name, item.Value),
                        Value = item.Value
                    }});
                }
            }

            return dictLstItemCatalog;
        }



        private static List<ItemCatalog> FillCategory(IEnumerable<CatalogsSvc.ItemCatalog> lstCatalog)
        {
            return lstCatalog.Select(item => new ItemCatalog {Id = item.Id, Name = item.Name}).ToList();
        }
    }
}

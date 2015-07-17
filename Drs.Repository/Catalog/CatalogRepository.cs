using System.Collections.Generic;
using System.Linq;
using Drs.Model.Shared;
using Drs.Repository.Shared;

namespace Drs.Repository.Catalog
{
    public class CatalogRepository : BaseOneRepository, ICatalogRepository
    {
        public IList<ItemCatalog> GetPayments()
        {
            return Db.CatPayment.Where(e => e.IsObsolete == false)
                .Select(e => new ItemCatalog
                {
                    Id = e.CatPaymentId,
                    Name = e.Name
                }).ToList();
        }

        public IList<ItemCatalog> GetDeliveryStatus()
        {
            return Db.CatDeliveryStatus.Where(e => e.IsObsolete == false)
                .Select(e => new ItemCatalog
                {
                    Id = e.CatDeliveryStatusId,
                    Code = e.Code,
                    Name = e.Name,
                    Value = e.Color
                }).ToList();
        }
    }
}
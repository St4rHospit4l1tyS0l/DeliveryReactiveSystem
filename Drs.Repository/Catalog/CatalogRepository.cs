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

        public IList<ItemCatalog> GetStores()
        {
            return Db.FranchiseStore.Where(e => e.Franchise.IsObsolete == false && e.IsObsolete == false)
                .Select(e => new ItemCatalog
                {
                    Id = e.FranchiseStoreId,
                    Code = e.Franchise.Code,
                    Name = e.Name,
                    SecondName = e.Franchise.Name,
                    Value = e.Address.MainAddress
                }).ToList();
        }

        public IList<ItemCatalog> GetUsersAgents()
        {
            return Db.UserDetail.Where(e => e.IsObsolete == false && e.AspNetUsers.AspNetRoles.Any(i => i.Name == "Agent"))
                .Select(e => new ItemCatalog
                {
                    Key = e.AspNetUsers.Id,
                    Name = e.FirstName + " " + e.LastName,
                    SecondName = e.AspNetUsers.UserName
                }).ToList();
        }
    }
}
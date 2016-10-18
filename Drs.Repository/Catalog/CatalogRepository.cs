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

        public Dictionary<string, List<MenuItem>> GetWebMenu()
        {
            var dicMenu = new Dictionary<string, List<MenuItem>>();
            
            var lstRel = Db.AspNetMenuRole.ToList();
            var lstMenu = Db.AspNetMenu.Where(e => e.IsObsolete == false)
                .OrderBy(e => e.Position)
                .Select(e => new MenuItem
                {
                    Id = e.AspNetMenuId,
                    Action = e.Action,
                    Area = e.Area,
                    Controller = e.Controller,
                    Icon = e.Icon,
                    Title = e.Title,
                    Position = e.Position,
                    ParentId = e.ParentId,
                    MenuName = e.MenuName,
                    HasToShow = e.HasToShow
                }).ToList();

            foreach (var role in Db.AspNetRoles.Select(e => new {e.Id, e.Name}))
            {
                var roleIn = role;
                var lstMenuRole = new List<MenuItem>();
                foreach (var rel in lstRel.Where(e => e.AspNetRoleId == roleIn.Id))
                {
                    var itemMenu = lstMenu.FirstOrDefault(e => e.Id == rel.AspNetMenuId);
                    if(itemMenu == null)
                        continue;

                    var copyMenu = itemMenu.CopyMenu();
                    lstMenuRole.Add(copyMenu);

                    if (itemMenu.ParentId == null) 
                        continue;
                    var menuParent = lstMenuRole.FirstOrDefault(e => e.Id == itemMenu.ParentId);
                    if(menuParent == null)
                        continue;
                    menuParent.SubMenu.Add(copyMenu);
                }

                var lstMenuRoots = lstMenuRole.Where(e => e.ParentId == null).ToList();

                if(lstMenuRoots.Any() == false)
                    continue;

                dicMenu.Add(role.Name, lstMenuRoots);
            }

            return dicMenu;
        }
    }
}
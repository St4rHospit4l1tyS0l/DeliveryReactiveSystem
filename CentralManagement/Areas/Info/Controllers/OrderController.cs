using System;
using System.Linq;
using System.Web.Mvc;
using Drs.Infrastructure.JqGrid.Model;
using Drs.Model.Constants;
using Drs.Model.Order;
using Drs.Model.Settings;
using Drs.Repository.Entities.Metadata;
using Drs.Repository.Log;
using Drs.Repository.Order;
using Drs.Repository.Shared;
using Newtonsoft.Json;

namespace CentralManagement.Areas.Info.Controllers
{
    [Authorize(Roles = RoleConstants.MANAGER + ", " + RoleConstants.INSTALLER)]
    public class OrderController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List(JqGridFilterModel opts)
        {
            using (var repository = new GenericRepository<Drs.Repository.Entities.ViewOrderInfo>())
            {
                var result = repository.JqGridFindBy(opts, OrderInfoJson.Key, OrderInfoJson.Columns, 
                    (e => !SettingsData.Constants.TrackConst.OrderStatusEnd.Contains(e.LastStatus)), OrderInfoJson.DynamicToDto);
                return Json(result);
            }
        }

        public ActionResult View(long id)
        {
            try
            {
                using (var repository = new OrderRepository())
                {
                    var model = repository.Db.OrderToStore.Where(e => e.OrderToStoreId == id)
                        .Select(e => new ViewInfoOrderModel
                        {
                            OrderToStoreId = e.OrderToStoreId,
                            FranchiseName = e.Franchise.Name,
                            FranchiseStoreName = e.FranchiseStore.Name,
                            StartDatetime = e.StartDatetime,
                            LastStatus = e.LastStatus,
                            Phone = e.ClientPhone.Phone,
                            FullName = e.Client.FirstName + " " + e.Client.LastName,
                            Address =
                                e.Address.RegionNameA + " " + e.Address.RegionNameB + " " + e.Address.RegionNameC + " " +
                                e.Address.MainAddress
                                + " " + e.Address.ExtIntNumber,
                            Reference = e.Address.Reference,
                            IsMap = e.Address.IsMap,
                            PlaceId = e.Address.PlaceId,
                            Lat = e.Address.Lat,
                            Lng = e.Address.Lng,
                            Total = e.PosOrder.Total,
                            UserNameIns = e.AspNetUsers.UserName,
                            Notes = e.ExtraNotes,
                            LstItems = e.PosOrder.PosOrderItem.Select(i => new InfoItemModel
                            {
                                PosOrderItemId = i.PosOrderItemId,
                                LevelItem = i.LevelItem,
                                Name = i.Name,
                                ParentId = i.ParentId ?? -1,
                                Price = i.Price
                            }).OrderBy(i => i.PosOrderItemId).ToList()
                        }).FirstOrDefault();

                    ViewBag.Model = JsonConvert.SerializeObject(model);
                }
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex, id);
            }

            return View();
        }
    }
}
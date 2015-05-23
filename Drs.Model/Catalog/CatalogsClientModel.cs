using System.Collections.Generic;
using Drs.Model.Constants;
using Drs.Model.Settings;
using Drs.Model.Shared;

namespace Drs.Model.Catalog
{
    public static class CatalogsClientModel
    {
        static CatalogsClientModel() 
        {
            //DicOrderStatus = new Dictionary<string, string>
            //{
            //    { OrderStatus.NEW_READY_TO_SEND, "Orden para enviar a la tienda" },
            //    { SettingsData.Constants.TrackConst.NONE, "Nueva orden" },
            //    { SettingsData.Constants.TrackConst.PRE_DELAY, "Nueva orden orden futura" },
            //    { SettingsData.Constants.TrackConst.IN_DELAY, "Orden retrasada (orden futura)" },
            //    { SettingsData.Constants.TrackConst.KITCHEN_DELAY, "Orden retrasada en cocina" },
            //    { SettingsData.Constants.TrackConst.COOKING, "Preparando orden en cocina" },
            //    { SettingsData.Constants.TrackConst.PREPARED, "Preparando la orden" },
            //    { SettingsData.Constants.TrackConst.IN_TRANSIT, "Orden en camino al cliente" },
            //    { SettingsData.Constants.TrackConst.FULFILLED, "Orden entregada al cliente" },
            //    { SettingsData.Constants.TrackConst.CLOSED, "Orden cerrada" }
            //};
        }

        public static List<ItemCatalog> CatPayments { get; set; }

        public static Dictionary<string, ItemCatalog> DicOrderStatus { get; set; }

    }
}

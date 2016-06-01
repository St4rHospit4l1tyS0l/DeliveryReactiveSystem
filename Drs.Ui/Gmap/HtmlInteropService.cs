using System;
using Drs.Infrastructure.Resources;
using Drs.Model.Address;
using Drs.ViewModel.Order;
using MahApps.Metro.Controls;
using Newtonsoft.Json;

namespace Drs.Ui.Gmap
{
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public class HtmlInteropService
    {
        private readonly Flyout _control;

        public HtmlInteropService(Flyout control)
        {
            _control = control;
        }

        public object GetInfoAddress()
        {
            var mv = _control.DataContext as UpsertAddressFoVm;

            if (mv == null)
                return "Configuración errónea";

            return JsonConvert.SerializeObject(new
            {
                mv.Controls,
                Address = mv.AddressMapInfo,
                mv.Franchise.LastConfig,
                mv.Franchise.StoresCoverage
            });
        }

        public object SaveAddress(string sValue)
        {
            try
            {
                var mv = _control.DataContext as UpsertAddressFoVm;

                if (mv == null)
                {
                    return JsonConvert.SerializeObject(new ResponseMessageModel
                    {
                        HasError = true,
                        Message = "La configuración del equipo es errónea"
                    });                    
                }

                var model = JsonConvert.DeserializeObject<AddressMapInfoModel>(sValue);
                mv.Save(model);

                return JsonConvert.SerializeObject(new ResponseMessageModel
                {
                    HasError = false,
                    Message = "Se ha almacenado de forma correcta la dirección"
                });

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ResponseMessageModel
                {
                    HasError = true,
                    Message = "Se presentó el siguiente error: " + ex.Message
                });
            }
        }
    }
}

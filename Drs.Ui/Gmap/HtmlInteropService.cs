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
                mv.Franchise.LastConfig,
                mv.Franchise.StoresCoverage
            });
        }
    }
}

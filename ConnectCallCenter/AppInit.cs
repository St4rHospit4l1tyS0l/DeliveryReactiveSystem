using Autofac;
using Drs.Model.Catalog;
using Drs.Repository.Address;
using Drs.Repository.Catalog;
using Drs.Repository.Setting;
using Drs.Service.Configuration;
using Drs.Service.Settings;
using Drs.ViewModel.Catalog;

namespace ConnectCallCenter
{
    public class AppInit
    {
        public static IContainer Container;
        public static void Start()
        {
            var bootstrapper = new Bootstrapper();
            Container = bootstrapper.Build();
            SettingAddress.Initialize(Container.Resolve<IAddressRepository>());
            SettingConfigure.Initialize(Container.Resolve<ISettingRepository>());
            CatalogsClientConfigure.Initialize(Container.Resolve<ICatalogRepository>());
        }
    }
}

using Autofac;
using Drs.Repository.Address;
using Drs.Repository.Setting;
using Drs.Service.Configuration;
using Drs.Service.Settings;

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

        }
    }
}

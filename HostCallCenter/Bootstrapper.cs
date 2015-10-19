using Autofac;
using Drs.Infrastructure.Logging;
using Drs.Repository.Account;
using Drs.Repository.Address;
using Drs.Repository.Catalog;
using Drs.Repository.Setting;
using Drs.Service.Account;
using Drs.Service.Address;
using Drs.Service.Catalogs;
using Drs.Service.Settings;
using ManagementCallCenter.Account;
using ManagementCallCenter.Catalogs;
using ManagementCallCenter.Setting;
using ManagementCallCenter.Sync;

namespace HostCallCenter
{
    public class Bootstrapper
    {
        public IContainer Build()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<LoginSvc>().As<ILoginSvc>();
            builder.RegisterType<SettingSvc>().As<ISettingSvc>();
            builder.RegisterType<CatalogsSvc>().As<ICatalogsSvc>();
            builder.RegisterType<SyncServerSvc>().As<ISyncServerSvc>();
            builder.RegisterType<DebugLoggerFactory>().As<ILoggerFactory>().SingleInstance();
            builder.RegisterType<AccountService>().As<IAccountService>();
            builder.RegisterType<AccountRepository>().As<IAccountRepository>();
            builder.RegisterType<AddressService>().As<IAddressService>();
            builder.RegisterType<AddressRepository>().As<IAddressRepository>();
            builder.RegisterType<SettingService>().As<ISettingService>();
            builder.RegisterType<SettingRepository>().As<ISettingRepository>();
            builder.RegisterType<CatalogService>().As<ICatalogService>();
            builder.RegisterType<CatalogRepository>().As<ICatalogRepository>();
            
            return builder.Build();
        }
    }
}
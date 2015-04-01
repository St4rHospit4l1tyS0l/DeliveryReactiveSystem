using Autofac;
using Drs.Infrastructure.Logging;
using Drs.Repository.Account;
using Drs.Repository.Address;
using Drs.Repository.Setting;
using Drs.Service.Account;
using Drs.Service.Address;
using Drs.Service.Settings;
using ManagementCallCenter.Account;
using ManagementCallCenter.Setting;

namespace HostCallCenter
{
    public class Bootstrapper
    {
        public IContainer Build()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<LoginSvc>().As<ILoginSvc>();
            builder.RegisterType<SettingSvc>().As<ISettingSvc>();
            builder.RegisterType<DebugLoggerFactory>().As<ILoggerFactory>().SingleInstance();
            builder.RegisterType<AccountService>().As<IAccountService>();
            builder.RegisterType<AccountRepository>().As<IAccountRepository>();
            builder.RegisterType<AddressService>().As<IAddressService>();
            builder.RegisterType<AddressRepository>().As<IAddressRepository>();
            builder.RegisterType<SettingService>().As<ISettingService>();
            builder.RegisterType<SettingRepository>().As<ISettingRepository>();
            
            return builder.Build();
        }
    }
}
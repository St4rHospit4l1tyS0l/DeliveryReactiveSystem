using Autofac;
using ConnectCallCenter.Hubs;
using Drs.Repository.Account;
using Drs.Repository.Address;
using Drs.Repository.Client;
using Drs.Repository.Order;
using Drs.Repository.Setting;
using Drs.Repository.Store;
using Drs.Repository.Track;
using Drs.Service.Account;
using Drs.Service.Address;
using Drs.Service.Client;
using Drs.Service.Order;
using Drs.Service.Settings;
using Drs.Service.Store;
using Drs.Service.Track;
using Drs.ViewModel;

namespace ConnectCallCenter
{
    public class Bootstrapper : BootstrapperBase
    {
        protected override void RegisterTypes(ContainerBuilder builder)
        {
            builder.RegisterType<AccountRepository>().As<IAccountRepository>();
            builder.RegisterType<AccountService>().As<IAccountService>();
            builder.RegisterType<ClientService>().As<IClientService>();
            builder.RegisterType<ClientRepository>().As<IClientRepository>();
            builder.RegisterType<AddressService>().As<IAddressService>();
            builder.RegisterType<AddressRepository>().As<IAddressRepository>();
            builder.RegisterType<FranchiseService>().As<IFranchiseService>();
            builder.RegisterType<FranchiseRepository>().As<IFranchiseRepository>();
            builder.RegisterType<OrderService>().As<IOrderService>();
            builder.RegisterType<OrderRepository>().As<IOrderRepository>();
            builder.RegisterType<SettingService>().As<ISettingService>();
            builder.RegisterType<SettingRepository>().As<ISettingRepository>();
            builder.RegisterType<StoreService>().As<IStoreService>();
            builder.RegisterType<StoreRepository>().As<IStoreRepository>();

            builder.RegisterType<TrackService>().As<ITrackService>();
            builder.RegisterType<TrackRepository>().As<ITrackRepository>();

            builder.RegisterType<OrderHub>().SingleInstance();
            builder.RegisterType<ClientHub>().SingleInstance();
            builder.RegisterType<AccountHub>().SingleInstance();
            builder.RegisterType<AddressHub>().SingleInstance();
            builder.RegisterType<StoreHub>().SingleInstance();
            builder.RegisterType<TrackHub>().SingleInstance();
            
        }
    }
}

using Autofac;
using Drs.Model.Menu;
using Drs.Service.Client;
using Drs.Service.Configuration;
using Drs.Service.Franchise;
using Drs.Service.Order;
using Drs.Service.ReactiveDelivery;
using Drs.Ui.Configuration;
using Drs.Ui.Ui;
using Drs.Ui.Ui.Order;
using Drs.ViewModel;
using Drs.ViewModel.Account;
using Drs.ViewModel.Main;
using Drs.ViewModel.Menu;
using Drs.ViewModel.Order;
using Drs.ViewModel.Shared;
using Drs.ViewModel.Track;
using MahApps.Metro.Controls;

namespace Drs.Ui
{
    public class Bootstrapper : BootstrapperBase
    {
        protected override void RegisterTypes(ContainerBuilder builder)
        {
            builder.RegisterType<ConfigurationProvider>().As<IConfigurationProvider>();
            builder.RegisterType<MainWindow>().SingleInstance();
            builder.RegisterType<LoginVm>().As<ILoginVm>().ExternallyOwned();
            builder.RegisterType<ReactiveDeliveryClient>().As<IReactiveDeliveryClient>().SingleInstance();
            //builder.RegisterType<CurrentUserSettings>().As<ICurrentUserSettings>().SingleInstance();

            builder.RegisterType<AutoCompleteTextVm>().As<IAutoCompleteTextVm>().ExternallyOwned();
            builder.RegisterType<AutoCompletePhoneVm>().As<IAutoCompletePhoneVm>().ExternallyOwned();
            builder.RegisterType<BackPreviousVm>().As<IBackPreviousVm>().ExternallyOwned();
            builder.RegisterType<MsgWndVm>().As<IMsgWndVm>().ExternallyOwned();

            builder.RegisterType<ButtonItemModel>().As<IButtonItemModel>().ExternallyOwned();
            builder.RegisterType<MenuItemVm>().As<IMenuItemVm>().ExternallyOwned();
            builder.RegisterType<MenuVm>().As<IMenuVm>().ExternallyOwned();

            builder.RegisterType<FranchiseService>().As<IFranchiseService>();
            builder.RegisterType<PosService>().As<IPosService>().SingleInstance();

            builder.RegisterType<MainOrderService>().As<IMainOrderService>().ExternallyOwned();
            builder.RegisterType<FranchiseVm>().As<IFranchiseVm>().ExternallyOwned();
            builder.RegisterType<OrderSummaryVm>().As<IOrderSummaryVm>().ExternallyOwned();
            builder.RegisterType<UpsertClientFoVm>().As<IUpsertClientFoVm>().ExternallyOwned();
            builder.RegisterType<UpsertAddressFoVm>().As<IUpsertAddressFoVm>().ExternallyOwned();
            builder.RegisterType<ClientsListVm>().As<IClientsListVm>().ExternallyOwned();
            builder.RegisterType<AddressListVm>().As<IAddressListVm>().ExternallyOwned();
            builder.RegisterType<FranchiseContainerVm>().As<IFranchiseContainerVm>().ExternallyOwned();
            builder.RegisterType<MainOrderVm>().As<IMainOrderVm>().ExternallyOwned();
            builder.RegisterType<SearchNewPhoneVm>().As<ISearchNewPhoneVm>().ExternallyOwned();
            builder.RegisterType<OrderPosVm>().As<IOrderPosVm>().ExternallyOwned();
            builder.RegisterType<SendOrderVm>().As<ISendOrderVm>().ExternallyOwned();
            builder.RegisterType<LastOrderFoVm>().As<ILastOrderFoVm>().ExternallyOwned();

            builder.RegisterType<TrackOrderVm>().As<ITrackOrderVm>().ExternallyOwned();
            builder.RegisterType<SearchTrackOrderVm>().As<ISearchTrackOrderVm>().ExternallyOwned();
            builder.RegisterType<OrdersListVm>().As<IOrdersListVm>().ExternallyOwned();
            builder.RegisterType<OrderDetailVm>().As<IOrderDetailVm>().ExternallyOwned();            

            builder.RegisterType<ShellContainerVm>().As<IShellContainerVm>().ExternallyOwned();
        }

        public override Flyout CreateFlyoutControl(IFlyoutBaseVm baseVm)
        {
            var name = baseVm.GetType().Name;
            if (name == typeof(UpsertClientFoVm).Name)
            {
                var view = new UpsertClientFo { DataContext = baseVm };
                return view;
            }
            
            if (name == typeof(UpsertAddressFoVm).Name)
            {
                var view = new UpsertAddressFo{ DataContext = baseVm };
                return view;
            }
            
            return null;
        }
    }
}


using Autofac;
using Drs.PosInterface.Configuration;
using Drs.PosService.BsLogic;
using Drs.Service.Configuration;
using Drs.Service.ReactiveDelivery;
using Drs.ViewModel;

namespace Drs.PosInterface
{
    public class Bootstrapper : BootstrapperBase
    {
        protected override void RegisterTypes(ContainerBuilder builder)
        {
            builder.RegisterType<ReactiveDeliveryClient>().As<IReactiveDeliveryClient>().SingleInstance();
            builder.RegisterType<ConfigurationProvider>().As<IConfigurationProvider>();
            builder.RegisterType<PosActService>().As<IPosActService>().SingleInstance();
        }
    }
}

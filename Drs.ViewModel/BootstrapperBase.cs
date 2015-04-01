using Autofac;
using Drs.Infrastructure.Logging;
using Drs.Repository.Account;
using Drs.ViewModel.Shared;
using MahApps.Metro.Controls;

namespace Drs.ViewModel
{
    public abstract class BootstrapperBase
    {
        public IContainer Build()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<DebugLoggerFactory>().As<ILoggerFactory>().SingleInstance();
            builder.RegisterType<AccountRepository>().As<IAccountRepository>().ExternallyOwned();

            RegisterTypes(builder);

            return builder.Build();
        }

        protected abstract void RegisterTypes(ContainerBuilder builder);

        public virtual Flyout CreateFlyoutControl(IFlyoutBaseVm baseVm)
        {
            return null;
        }

    }
}

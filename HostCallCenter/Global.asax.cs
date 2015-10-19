using System;
using Autofac;
using Autofac.Integration.Wcf;
using Drs.Repository.Address;
using Drs.Service.Configuration;
using Drs.Service.Settings;

namespace HostCallCenter
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            var container = new Bootstrapper().Build();
            AutofacHostFactory.Container = container;
            SettingAddress.Initialize(container.Resolve<IAddressRepository>());
            InitializeSettingsService.InitializeConstants();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}
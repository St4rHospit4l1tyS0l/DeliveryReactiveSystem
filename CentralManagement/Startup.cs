using Drs.Service.Account;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CentralManagement.Startup))]
namespace CentralManagement
{
    public partial class Startup
    {
         public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            new AccountService().ValidateMainAccount();
        }
    }
}

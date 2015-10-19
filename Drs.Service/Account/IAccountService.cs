using System.Collections.Generic;
using System.Threading.Tasks;
using Drs.Infrastructure.Resources;
using Drs.Model.Account;
using Drs.Model.Franchise;
using Drs.Model.Menu;
using Drs.Model.Shared;

namespace Drs.Service.Account
{
    public interface IAccountService
    {
        ResponseMessage Login(LoginModel login);
        IEnumerable<ButtonItemModel> GetMenuByUser(string username);
        string GetAccountInfo(string s, string s1);
        DeviceInfoModel GetLstDevices();
        bool DoSelectServer(int id, bool enable);
        bool DoSelectClient(int id, bool enable);
        Task<ResponseMessageModel> AskForLicense();
        string GetActivationCodeToShow();
    }
}

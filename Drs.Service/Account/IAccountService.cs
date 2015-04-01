using System.Collections.Generic;
using Drs.Model.Account;
using Drs.Model.Menu;
using Drs.Model.Shared;

namespace Drs.Service.Account
{
    public interface IAccountService
    {
        ResponseMessage Login(LoginModel login);
        IEnumerable<ButtonItemModel> GetMenuByUser(string username);
    }
}

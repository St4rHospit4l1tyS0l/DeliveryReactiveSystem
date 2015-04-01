using System;
using System.Collections.Generic;
using Drs.Model.Account;
using Drs.Model.Menu;

namespace Drs.Repository.Account
{
    public interface IAccountRepository : IDisposable
    {
         UserSalt GetUserSalt(string username);
        bool IsUserIdAndPasswordCorrect(string userId, string hashPassword);
        string GetRoleIdByUsername(string username);
        IEnumerable<ButtonItemModel> GetMenuByRole(string roleId);
    }
}

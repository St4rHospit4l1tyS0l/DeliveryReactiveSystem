using System;
using System.Collections.Generic;
using Drs.Infrastructure.Model;
using Drs.Model.Account;
using Drs.Model.Menu;
using Drs.Repository.Entities;

namespace Drs.Repository.Account
{
    public interface IAccountRepository : IDisposable
    {
         UserSalt GetUserSalt(string username);
        bool IsUserIdAndPasswordCorrect(string userId, string hashPassword);
        string GetRoleIdByUsername(string username);
        IEnumerable<ButtonItemModel> GetMenuByRole(string roleId);
        string GetComputerInfo(string eInfo);
        void AddComputerInfo(string eInfo, string clientCode);
        void AddServerInfo(string eInfo, string serverCode);
        ServerInfo GetServerInfo(string eInfo);
        void SaveChanges();
        IEnumerable<ConnectionFullModel> GetLstClients();
        IEnumerable<ConnectionFullModel> GetLstServers();
        bool ExistsServer(int id);
        int GetCallCenterId();
        int AddCallCenterId();
        ServerInfo GetServerInfo(int id);
        ClientInfo GetClientInfo(int id);
        List<string> GetLstClientsCodes();
        List<string> GetLstServersCodes();
        void AddActivationCode(string code);
        void UpdateActivationCode(string code);
        string GetActivationCode();
        ClientInfo GetClientByHostName(string hn);
        ServerInfo GetServerByHostName(string hn);
        void UpdateComputerInfo(string eInfo, string decCompInfo);
    }
}

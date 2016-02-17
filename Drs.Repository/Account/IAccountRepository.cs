using System;
using System.Collections.Generic;
using Drs.Infrastructure.Model;
using Drs.Model.Account;
using Drs.Model.Franchise;
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
        void AddInfoServer(string eInfo, string serverCode);
        InfoServer GetInfoServer(string eInfo);
        void SaveChanges();
        IEnumerable<ConnectionFullModel> GetLstClients();
        ConnectionFullModel GetClient(int id);
        IEnumerable<ConnectionFullModel> GetLstServers();
        bool ExistsServer(int id);
        int GetCallCenterId();
        int AddCallCenterId();
        InfoServer GetInfoServer(int id);
        InfoClientTerminal GetInfoClientTerminal(int id);
        List<string> GetLstClientsCodes();
        List<string> GetLstServersCodes();
        void AddActivationCode(string code);
        void UpdateActivationCode(string code);
        string GetActivationCode();
        InfoClientTerminal GetClientTerminalByHost(string hn);
        InfoServer GetServerByHost(string hn);
        void UpdateComputerInfo(string eInfo, string decCompInfo);
        List<TerminaFranchiseModel> GetLstTerminalFranchise(int id);
        int UpsertTerminalFranchise(TerminaFranchiseModel model);
    }
}

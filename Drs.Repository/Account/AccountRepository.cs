using System.Collections.Generic;
using System.Linq;
using Drs.Infrastructure.Model;
using Drs.Model.Account;
using Drs.Model.Menu;
using Drs.Repository.Entities;
using Drs.Repository.Shared;
using ReactiveUI;

namespace Drs.Repository.Account
{
    public class AccountRepository : BaseOneRepository, IAccountRepository
    {

        public UserSalt GetUserSalt(string username)
        {
            return DbEntities.AspNetUsers
                .Where(e => e.UserName.Trim().ToLower() == username.Trim().ToLower())
                .Select(e => new UserSalt{UserId = e.Id})
                .FirstOrDefault();
        }
        public bool IsUserIdAndPasswordCorrect(string userId, string hashPassword)
        {
            return DbEntities.AspNetUsers
                .Any(e => e.Id == userId && e.PasswordHash == hashPassword);
        }

        public string GetRoleIdByUsername(string username)
        {
            return DbEntities.AspNetUsers.Where(e => e.UserName.Trim().ToLower() == username.Trim().ToLower())
                .Select(e => e.AspNetRoles.Select(i => i.Id).FirstOrDefault()).Single();
        }
        public static string GetIdByUsername(string username, CallCenterEntities dbEntities)
        {
            return dbEntities.AspNetUsers.Where(e => e.UserName.Trim().ToLower() == username.Trim().ToLower())
                .Select(e => e.Id).Single();
        }

        public IEnumerable<ButtonItemModel> GetMenuByRole(string roleId)
        {
            return DbEntities.AspNetRoles.Join(DbEntities.Permission, pr => pr.Id, pe => pe.RoleId,
                    (pr, pe) => new {pe.ModuleId, pr.Id})
                    .Join(DbEntities.Module, per => per.ModuleId, mo => mo.ModuleId,
                        (per, mo) =>
                            new
                            {
                                per.Id,
                                mo.Name,
                                mo.MenuModule.Color,
                                mo.MenuModule.Position,
                                mo.MenuModule.Image,
                                mo.IsObsolete,
                                mo.Code
                            })
                    .Where(e => e.Id == roleId && e.IsObsolete == false).Select(e => new ButtonItemModel
                    {
                        Color = e.Color,
                        Title = e.Name,
                        Position = e.Position,
                        Image = e.Image,
                        Code = e.Code
                    }).OrderBy(e => e.Position).ToList();
        }

        public string GetComputerInfo(string eInfo)
        {
            return DbEntities.ClientInfo.Where(e => e.ClientHost == eInfo).Select(e => e.ClientCode).FirstOrDefault();
        }

        public void AddComputerInfo(string clientHost, string clientCode)
        {
            DbEntities.ClientInfo.Add(new ClientInfo
            {
                ClientHost = clientHost,
                ClientCode = clientCode
            });

            DbEntities.SaveChanges();
        }

        public void AddServerInfo(string eInfo, string serverCode)
        {
            DbEntities.ServerInfo.Add(new ServerInfo
            {
                ServerName = eInfo,
                ServerCode = serverCode
            });

            DbEntities.SaveChanges();            
        }

        public ServerInfo GetServerInfo(string eInfo)
        {
            return DbEntities.ServerInfo.FirstOrDefault(e => e.ServerName == eInfo);
        }

        public void SaveChanges()
        {
            DbEntities.SaveChanges();
        }

        public IEnumerable<ConnectionFullModel> GetLstClients()
        {
            return DbEntities.ClientInfo.Select(e => new ConnectionFullModel
            {
                DeviceId = e.ClientInfoId,
                Code = e.ClientCode,
                IsSelected = e.CallCenterInfoId.HasValue
            }).ToList();
        }

        public IEnumerable<ConnectionFullModel> GetLstServers()
        {
            return DbEntities.ServerInfo.Select(e => new ConnectionFullModel
            {
                DeviceId = e.ServerInfoId,
                Code = e.ServerCode,
                IsSelected = e.CallCenterInfoId.HasValue
            }).ToList();
        }

        public bool ExistsServer(int id)
        {
            return DbEntities.ServerInfo.Any(e => e.ServerInfoId == id);
        }

        public int GetCallCenterId()
        {
            return DbEntities.CallCenterInfo.Select(e => e.CallCenterInfoId).FirstOrDefault();
        }

        public int AddCallCenterId()
        {
            var callCenter = new CallCenterInfo();
            DbEntities.CallCenterInfo.Add(callCenter);
            DbEntities.SaveChanges();
            return callCenter.CallCenterInfoId;
        }

        public ServerInfo GetServerInfo(int id)
        {
           return DbEntities.ServerInfo.SingleOrDefault(e => e.ServerInfoId == id);
        }

        public ClientInfo GetClientInfo(int id)
        {
            return DbEntities.ClientInfo.SingleOrDefault(e => e.ClientInfoId == id);
        }

        public List<string> GetLstClientsCodes()
        {
            return DbEntities.ClientInfo.Where(e => e.CallCenterInfoId != null).Select(e => e.ClientCode).ToList();
        }

        public List<string> GetLstServersCodes()
        {
            return DbEntities.ServerInfo.Where(e => e.CallCenterInfoId != null).Select(e => e.ServerCode).ToList();
        }

        public void AddActivationCode(string code)
        {
            var callCenter = new CallCenterInfo { ActivationCode = code };
            DbEntities.CallCenterInfo.Add(callCenter);
            DbEntities.SaveChanges();            
        }

        public void UpdateActivationCode(string code)
        {
            var callCenter = DbEntities.CallCenterInfo.FirstOrDefault();

            if (callCenter == null){
                AddActivationCode(code);
                return;
            }

            callCenter.ActivationCode = code;
            DbEntities.SaveChanges();            
        }

        public string GetActivationCode()
        {
            return DbEntities.CallCenterInfo.Select(e => e.ActivationCode).FirstOrDefault();
        }

        public bool IsValidUser(string id)
        {
            return DbEntities.UserDetail.Any(e => e.Id == id && e.IsObsolete == false);
        }

    }
}

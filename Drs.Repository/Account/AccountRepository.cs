using System.Collections.Generic;
using System.Linq;
using Drs.Infrastructure.Model;
using Drs.Model.Account;
using Drs.Model.Constants;
using Drs.Model.Menu;
using Drs.Model.Shared;
using Drs.Repository.Entities;
using Drs.Repository.Shared;

namespace Drs.Repository.Account
{
    public class AccountRepository : BaseOneRepository, IAccountRepository
    {
        public AccountRepository()
        {
            
        }

        public AccountRepository(CallCenterEntities db)
            :base(db)
        {
            
        }

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
            return DbEntities.InfoClientTerminal.Where(e => e.Host == eInfo).Select(e => e.Code).FirstOrDefault();
        }

        public void AddComputerInfo(string clientHost, string clientCode)
        {
            DbEntities.InfoClientTerminal.Add(new InfoClientTerminal
            {
                Host = clientHost,
                Code = clientCode
            });

            DbEntities.SaveChanges();
        }

        public void AddInfoServer(string eInfo, string serverCode)
        {
            DbEntities.InfoServer.Add(new InfoServer
            {
                Name = eInfo,
                Code = serverCode
            });

            DbEntities.SaveChanges();            
        }

        public InfoServer GetInfoServer(string eInfo)
        {
            return DbEntities.InfoServer.FirstOrDefault(e => e.Name == eInfo);
        }

        public void SaveChanges()
        {
            DbEntities.SaveChanges();
        }

        public IEnumerable<ConnectionFullModel> GetLstClients()
        {
            return DbEntities.InfoClientTerminal.Select(e => new ConnectionFullModel
            {
                DeviceId = e.InfoClientTerminalId,
                Code = e.Code,
                IsSelected = e.InfoCallCenterId.HasValue
            }).ToList();
        }

        public IEnumerable<ConnectionFullModel> GetLstServers()
        {
            return DbEntities.InfoServer.Select(e => new ConnectionFullModel
            {
                DeviceId = e.InfoServerId,
                Code = e.Code,
                IsSelected = e.InfoCallCenterId.HasValue
            }).ToList();
        }

        public bool ExistsServer(int id)
        {
            return DbEntities.InfoServer.Any(e => e.InfoServerId == id);
        }

        public int GetCallCenterId()
        {
            return DbEntities.InfoCallCenter.Select(e => e.InfoCallCenterId).FirstOrDefault();
        }

        public int AddCallCenterId()
        {
            var callCenter = new InfoCallCenter();
            DbEntities.InfoCallCenter.Add(callCenter);
            DbEntities.SaveChanges();
            return callCenter.InfoCallCenterId;
        }

        public InfoServer GetInfoServer(int id)
        {
           return DbEntities.InfoServer.SingleOrDefault(e => e.InfoServerId == id);
        }

        public InfoClientTerminal GetInfoClientTerminal(int id)
        {
            return DbEntities.InfoClientTerminal.SingleOrDefault(e => e.InfoClientTerminalId == id);
        }

        public List<string> GetLstClientsCodes()
        {
            return DbEntities.InfoClientTerminal.Where(e => e.InfoCallCenterId != null).Select(e => e.Code).ToList();
        }

        public List<string> GetLstServersCodes()
        {
            return DbEntities.InfoServer.Where(e => e.InfoCallCenterId != null).Select(e => e.Code).ToList();
        }

        public void AddActivationCode(string code)
        {
            var callCenter = new InfoCallCenter { ActivationCode = code };
            DbEntities.InfoCallCenter.Add(callCenter);
            DbEntities.SaveChanges();            
        }

        public void UpdateActivationCode(string code)
        {
            var callCenter = DbEntities.InfoCallCenter.FirstOrDefault();

            if (callCenter == null){
                AddActivationCode(code);
                return;
            }

            callCenter.ActivationCode = code;
            DbEntities.SaveChanges();            
        }

        public string GetActivationCode()
        {
            return DbEntities.InfoCallCenter.Select(e => e.ActivationCode).FirstOrDefault();
        }

        public InfoClientTerminal GetClientTerminalByHost(string hn)
        {
            return DbEntities.InfoClientTerminal.FirstOrDefault(e => e.Host == hn);
        }

        public InfoServer GetServerByHost(string hn)
        {
            return DbEntities.InfoServer.FirstOrDefault(e => e.Name == hn);
        }

        public void UpdateComputerInfo(string hn, string sCode)
        {
            var computerInfo = DbEntities.InfoClientTerminal.FirstOrDefault(e => e.Host == hn);
            
            if (computerInfo == null)
                return;

            computerInfo.Code = sCode;
            DbEntities.SaveChanges();
        }

        public bool IsValidUser(string id)
        {
            return DbEntities.UserDetail.Any(e => e.Id == id && e.IsObsolete == false);
        }

        public IList<OptionModel> GetManagerStoreUsers()
        {
            return DbEntities.AspNetUsers.Where(e => e.UserDetail.IsObsolete == false &&
                e.AspNetRoles.Any(i => i.Name == RoleConstants.STORE_MANAGER))
                .Select(e => new OptionModel
                {
                    StKey = e.Id,
                    Name = e.UserName
                }).ToList();
        }

        public string GetRoleDescByUsername(string userName)
        {
            return DbEntities.AspNetUsers.Where(e => e.UserName == userName)
                .Select(e => e.AspNetRoles.Select(i => i.Name).FirstOrDefault()).FirstOrDefault();
        }

    }
}

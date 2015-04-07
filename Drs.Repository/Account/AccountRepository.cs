using System.Collections.Generic;
using System.Linq;
using Drs.Model.Account;
using Drs.Model.Menu;
using Drs.Repository.Entities;
using Drs.Repository.Shared;

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

        public void AddComputerInfo(string eInfo, string encrypt)
        {
            throw new System.NotImplementedException();
        }

        public bool IsValidUser(string id)
        {
            return DbEntities.UserDetail.Any(e => e.Id == id && e.IsObsolete == false);
        }
    }
}

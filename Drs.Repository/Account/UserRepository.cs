using System;
using System.Linq;
using Drs.Repository.Entities;
using Drs.Repository.Entities.Metadata;
using Drs.Repository.Shared;

namespace Drs.Repository.Account
{
    public class UserRepository : BaseRepository
    {
        public UserRepository()
            : base(new CallCenterEntities())
        {
        }

        public UserRepository(CallCenterEntities dbConn)
            : base(dbConn)
        {
        }

        public UserInfoDto FindViewById(string id)
        {
            return DbConn.UserDetail.Where(e => e.Id == id)
                .Select(e => new UserInfoDto
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.AspNetUsers.Email,
                    IsObsolete = e.IsObsolete,
                    RoleId = e.AspNetUsers.AspNetRoles.Select(i => i.Id).FirstOrDefault(),
                    UserName = e.AspNetUsers.UserName,
                    PhoneNumber = e.AspNetUsers.PhoneNumber,
                    BirthDate = e.BirthDate
                }).FirstOrDefault();
        }

        public bool IsAlreadyUser(string userName)
        {
            return DbConn.AspNetUsers.Any(e => e.UserName.Trim().ToLower() == userName.Trim().ToLower());
        }

        public bool IsAlreadyUser(string id, string userName)
        {
            return DbConn.AspNetUsers.Any(e => e.Id != id && e.UserName.Trim().ToLower() == userName.Trim().ToLower());
        }

        public void AddUser(ViewUserInfoModel model)
        {
            DbConn.UserDetail.Add(new UserDetail
            {
                Id = model.Id,
                BirthDate = model.BirthDateIn,
                FirstName = model.FirstName,
                LastName = model.LastName,
                IsObsolete = false,
                InsDateTime = DateTime.Now
            });
            DbConn.SaveChanges();
        }

        public bool UpdateUser(ViewUserInfoModel model)
        {
            if (IsAlreadyUser(model.Id, model.UserName))
                return false;

            var userOld = DbConn.AspNetUsers.FirstOrDefault(e => e.Id == model.Id);
            var userDetailOld = DbConn.UserDetail.FirstOrDefault(e => e.Id == model.Id);

            if (userOld == null ||  userDetailOld == null)
                return false;

            userOld.Email = model.Email;
            userOld.UserName = model.UserName;

            userDetailOld.BirthDate = model.BirthDateIn;
            userDetailOld.FirstName = model.FirstName;
            userDetailOld.LastName = model.LastName;
            userDetailOld.IsObsolete = model.IsObsoleteIn;

            if (userOld.AspNetRoles.Any(e => e.Id == model.RoleId) == false)
            {
                var newRole = DbConn.AspNetRoles.FirstOrDefault(e => e.Id == model.RoleId);

                if (newRole == null)
                    return false;

                for (var i = userOld.AspNetRoles.Count-1; i >= 0; i--)
                {
                    var role = userOld.AspNetRoles.ElementAt(i);
                    userOld.AspNetRoles.Remove(role);
                }

                userOld.AspNetRoles.Add(newRole);
                DbConn.SaveChanges();
            }

            DbConn.SaveChanges();
            return true;
        }

        public void EnableUser(string id, bool bEnabled)
        {
            var userOld = DbConn.UserDetail.FirstOrDefault(e => e.Id == id);

            if (userOld == null)
                return;

            userOld.IsObsolete = !bEnabled;
            DbConn.SaveChanges();
        }

        public bool UserExistsById(string id)
        {
            return DbConn.AspNetUsers.Any(e => e.Id == id);
        }

        public string[] RolesByUserId(string id)
        {
            var lstRoles = DbConn.AspNetUsers.Where(e => e.Id == id).Select(e => e.AspNetRoles.Select(i => i.Description)).ToList();
            return lstRoles.SelectMany(e => e).ToArray();
        }
    }
}

using System;

namespace Drs.Repository.Entities.Metadata
{
    public class UserInfoDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName {
            get
            {
                return String.Format("{0} {1}", FirstName, LastName);
            }
        }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsObsolete { get; set; }

        public string IsObsoleteTx
        {
            get
            {
                return IsObsolete ? "SI" : "NO";
            }
        }

        public string Role { get; set; }
        public string RoleId { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? BirthDate { get; set; }

        public static ViewUserInfoModel ToDto(UserInfoDto data)
        {
            return new ViewUserInfoModel
            {
                Email = data.Email,
                FirstName = data.FirstName,
                Id = data.Id,
                IsObsolete = data.IsObsolete,
                LastName = data.LastName,
                RoleId = data.RoleId,
                UserName = data.UserName,
                BirthDate = data.BirthDate,
                PhoneNumber = data.PhoneNumber,
            };
        }
    }
}
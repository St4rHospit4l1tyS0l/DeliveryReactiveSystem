using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Drs.Model.Constants;

namespace Drs.Repository.Entities.Metadata
{
    public class ViewUserInfoModel
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(200)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "La contraseña y la confirmación no coinciden")]
        public string Confirm { get; set; }

        [Required]
        [MaxLength(250)]
        [EmailAddress]
        public string Email { get; set; }

        public bool IsObsolete { get; set; }

        [Required]
        public string RoleId { get; set; }

        [MaxLength(250)]
        public string PhoneNumber { get; set; }

        public DateTime? BirthDate { get; set; }
        public DateTime BirthDateIn
        {
            get
            {
                if (String.IsNullOrWhiteSpace(BirthDateTxIn))
                    return DateTime.Today.AddYears(-20);

                return DateTime.ParseExact(BirthDateTxIn, SharedConstants.DATE_FORMAT, CultureInfo.InvariantCulture,
                    DateTimeStyles.None);
            }
        }
        public string BirthDateTx
        {
            get
            {
                if (BirthDate == null)
                    BirthDate = DateTime.Now.AddYears(-20);

                return BirthDate.Value.ToString(SharedConstants.DATE_FORMAT);

            }
        }

        public string BirthDateTxIn { get; set; }

        public string IsObsoleteTx
        {
            get
            {
                return IsObsolete ? "true" : "false";
            }
        }

        public string IsObsoleteTxIn { get; set; }
        public bool IsObsoleteIn
        {
            get
            {
                return !String.IsNullOrWhiteSpace(IsObsoleteTxIn) && IsObsoleteTxIn.Trim().ToLower() == "on";
            }
        }

    }
}

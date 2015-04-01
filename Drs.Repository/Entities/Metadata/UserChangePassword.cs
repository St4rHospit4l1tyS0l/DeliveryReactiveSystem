using System.ComponentModel.DataAnnotations;

namespace Drs.Repository.Entities.Metadata
{
    public class UserChangePassword
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "La contraseña y la confirmación no coinciden")]
        public string Confirm { get; set; }

    }
}

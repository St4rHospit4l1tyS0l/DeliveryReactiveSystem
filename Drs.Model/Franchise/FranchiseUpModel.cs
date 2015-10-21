using System.ComponentModel.DataAnnotations;
using Drs.Model.Shared;

namespace Drs.Model.Franchise
{
    public class FranchiseUpModel
    {
        public int FranchiseId { get; set; }

        [Display(Name = @"Nombre corto")]
        [Required(AllowEmptyStrings = false)]
        public string ShortName { get; set; }

        [Display(Name = @"Franquicia")]
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Display(Name = @"Código")]
        [Required(AllowEmptyStrings = false)]
        public string Code { get; set; }

        [Display(Name = @"Directorio Aloha DATA")]
        [Required(AllowEmptyStrings = false)]
        public string DataFolder { get; set; }

        [Display(Name = @"Directorio Aloha NEWDATA")]
        [Required(AllowEmptyStrings = false)]
        public string NewDataFolder { get; set; }

        [Display(Name = @"URL Web Service")]
        [Required(AllowEmptyStrings = false)]
        public string WsAddress { get; set; }

        [Display(Name = @"Posición")]
        public int Position { get; set; }

        [Display(Name = @"Color del botón")]
        [Required(AllowEmptyStrings = false)]
        public string Color { get; set; }

        //[Display(Name = @"Nombre de la imagen")]
        //[Required(AllowEmptyStrings = false)]
        //public string Image { get; set; }
        [Required]
        public ResourceModel Resource { get; set; }

        [Display(Name = @"Tipo de productos")]
        [Required(AllowEmptyStrings = false)]
        public string Products { get; set; }

        public string UserInsUpId { get; set; }
    }
}

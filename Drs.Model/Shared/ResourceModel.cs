using System.ComponentModel.DataAnnotations;

namespace Drs.Model.Shared
{
    public class ResourceModel
    {
        [Display(Name = @"Imagen")]
        [Required(AllowEmptyStrings = false)]
        public string UidFileName { get; set; }
    }
}

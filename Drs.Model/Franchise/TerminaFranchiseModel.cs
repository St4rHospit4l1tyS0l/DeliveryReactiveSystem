using System.ComponentModel.DataAnnotations;

namespace Drs.Model.Franchise
{
    public class TerminaFranchiseModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El identificador de la terminal es un campo requerido")]
        public int InfoClientTerminalId { get; set; }

        [Required(ErrorMessage = "El identificador de la franquicia es un campo requerido")]
        public int FranchiseId { get; set; }
        public string FranchiseCode { get; set; }
        public string FranchiseName { get; set; }

        [Required(ErrorMessage = "La IP del POS es un campo requerido")]
        [MinLength(7, ErrorMessage = "Longitud mínima 7 caracteres")]
        [MaxLength(15, ErrorMessage = "Longitud máxima 15 caracteres")]
        public string Ip { get; set; }
    }
}

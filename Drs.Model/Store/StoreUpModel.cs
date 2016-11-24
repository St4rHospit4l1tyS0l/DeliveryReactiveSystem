using System.ComponentModel.DataAnnotations;

namespace Drs.Model.Store
{
    public class StoreUpModel
    {
        public int FranchiseStoreId { get; set; }
        public int FranchiseId { get; set; }
        public string ManUserId { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = @"Nombre de la sucursal es requerido")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = @"La URL del web service es requerida")]
        public string WsAddress { get; set; }
        public long AddressId { get; set; }
        public AddressModel Address { get; set; }
        public AddressModel AddressRes { get; set; }
        public string UserInsUpId { get; set; }
    }
}
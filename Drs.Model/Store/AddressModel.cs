using System.ComponentModel.DataAnnotations;

namespace Drs.Model.Store
{
    public class AddressModel
    {
        public int? CountryId { get; set; }
        public int? ZipCodeId { get; set; }
        public int? RegionArId { get; set; }
        public int? RegionBrId { get; set; }
        public int? RegionCrId { get; set; }
        public int? RegionDrId { get; set; }
        
        [Required]
        public string MainAddress { get; set; }
        
        [Required]
        public string NumExt { get; set; }
        public string Reference { get; set; }
        public string Country { get; set; }
        public string RegionA { get; set; }
        public string RegionB { get; set; }
        public string RegionC { get; set; }
        public string RegionD { get; set; }
        public string ZipCode { get; set; }
    }
}
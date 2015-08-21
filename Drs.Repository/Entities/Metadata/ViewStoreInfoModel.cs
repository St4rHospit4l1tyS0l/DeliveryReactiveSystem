using System.ComponentModel.DataAnnotations;

namespace Drs.Repository.Entities.Metadata
{
    public class ViewStoreInfoModel
    {
        public int FranchiseStoreId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Name { get; set; }

        [Required]
        public string FranchiseId { get; set; }

    }
}
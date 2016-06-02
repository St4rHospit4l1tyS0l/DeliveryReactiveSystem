using System.Data.Entity.Spatial;

namespace Drs.Model.Address
{
    public class CoverageStoreModel
    {
        public int StoreId { get; set; }
        public DbGeography Coverage { get; set; }
    }
}
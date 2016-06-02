using System.Collections.Generic;
using System.Data.Entity.Spatial;

namespace Drs.Model.Address
{
    public class SetCoverageStoreModel
    {
        public int StoreId { get; set; }
        public List<DbGeography> Coverage { get; set; }
    }
}
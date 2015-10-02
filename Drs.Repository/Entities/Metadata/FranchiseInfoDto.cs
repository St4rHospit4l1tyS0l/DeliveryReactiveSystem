using System;

namespace Drs.Repository.Entities.Metadata
{
    public class FranchiseInfoDto
    {
        public int FranchiseId { get; set; }
        public String Name { get; set; }
        public String Code { get; set; }
        public String UserNameIns { get; set; }
        public int Position { get; set; }
    }
}
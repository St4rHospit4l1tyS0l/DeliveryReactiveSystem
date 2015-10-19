using System;
using System.Collections.Generic;

namespace Drs.Model.Franchise
{
    public class SyncFranchiseModel
    {
        public int FranchiseId { get; set; }

        public string Code { get; set; }

        public List<SyncFileModel> LstFiles { get; set; }
        public string Version { get; set; }
        public int FranchiseDataVersionId { get; set; }
        public Guid FranchiseDataVersionUid { get; set; }
    }
}
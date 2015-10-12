using System;

namespace Drs.Model.Franchise
{
    public class UnSyncListModel
    {
        public int FranchiseId { get; set; }
        public int FranchiseDataVersionId { get; set; }
        public string WsAddress { get; set; }
        public Guid FranchiseDataVersionUid { get; set; }
    }
}
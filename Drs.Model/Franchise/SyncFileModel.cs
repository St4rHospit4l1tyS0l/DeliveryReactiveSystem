using Drs.Model.Shared;

namespace Drs.Model.Franchise
{
    public class SyncFileModel
    {
        public int FranchiseDataFileId { get; set; }
        
        public string FileName { get; set; }

        public string CheckSum { get; set; }

        public int FileType { get; set; }

        public bool HasError { get; set; }

        public string Message { get; set; }
    }
}
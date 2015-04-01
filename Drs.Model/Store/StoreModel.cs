using System.Collections.Generic;
using Drs.Model.Shared;

namespace Drs.Model.Store
{
    public class StoreModel : ListItemModel
    {
        public string MainAddress { get; set; }
        public List<string> LstPhones { get; set; }
        public string WsAddress { get; set; }
    }
}

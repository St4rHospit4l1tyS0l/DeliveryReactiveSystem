using System;

namespace Drs.Infrastructure.Model
{
    public class ConnectionInfoModel
    {
        public string Hk { get; set; }
        public string Hn { get; set; }
        public DateTime St { get; set; }
        public DateTime Et { get; set; }
        public bool Iv { get; set; }
        public int Code { get; set; }
    }
}

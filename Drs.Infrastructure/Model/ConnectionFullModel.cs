namespace Drs.Infrastructure.Model
{
    public class ConnectionFullModel
    {
        public string Code { get; set; }
        public bool IsSelected { get; set; }
        public string DeviceName { get; set; }
        public string StartDateTx { get; set; }
        public string EndDateTx { get; set; }
        public bool IsValid { get; set; }
        public int CodeId { get; set; }
        public int DeviceId { get; set; }
    }
}
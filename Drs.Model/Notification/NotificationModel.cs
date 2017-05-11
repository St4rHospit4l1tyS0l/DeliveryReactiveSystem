namespace Drs.Model.Notification
{
    public class NotificationModel
    {
        public string Message { get; set; }
        public int CategoryMessageId { get; set; }
        public int FranchiseStoreId { get; set; }
        public bool IsIndefinite { get; set; }
        public string Resource { get; set; }
    }
}

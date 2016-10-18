using System.ComponentModel.DataAnnotations;

namespace Drs.Model.Store
{
    public class StoreNotificationModel
    {
        private string _notification;

        [Required]
        public int FranchiseStoreId { get; set; }

        [Required]
        public int CategoryMessageId { get; set; }

        [Required]
        public string Notification
        {
            get { return _notification; }
            set
            {
                _notification = value;
                if (_notification != null)
                    _notification = _notification.Trim().ToUpperInvariant();
            }
        }
    }
}
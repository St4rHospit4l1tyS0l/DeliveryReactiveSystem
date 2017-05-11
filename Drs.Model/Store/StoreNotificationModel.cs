using System.ComponentModel.DataAnnotations;

namespace Drs.Model.Store
{
    public class StoreNotificationModel
    {
        private string _message;

        [Required]
        public int FranchiseStoreId { get; set; }

        [Required]
        public int CategoryMessageId { get; set; }

        [Required]
        public bool IsIndefinite { get; set; }

        public string Resource { get; set; }

        [Required]
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                if (_message != null)
                    _message = _message.Trim().ToUpperInvariant();
            }
        }
    }
}
using System.Collections.Generic;

namespace Drs.Model.Store
{
    public class StoreNotificationCategoryViewModel : StoreNotificationCategoryModel
    {
        public int ItemsCount
        {
            get
            {
                return NotificationsVm == null ? 0 : NotificationsVm.Count;
            }
        }
        
        public List<MessageNotificationVm> NotificationsVm { get; set; }

    }
}
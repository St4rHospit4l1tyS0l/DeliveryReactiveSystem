using System.Collections.Generic;

namespace Drs.Model.Store
{
    public class StoreNotificationCategoryModel
    {
        public string CategoryName { get; set; }
        public string Color { get; set; }
        public int Position { get; set; }
        public List<MessageNotification> Notifications { get; set; }

    }
}
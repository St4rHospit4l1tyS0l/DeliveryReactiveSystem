using System.Collections.Generic;

namespace Drs.Model.Store
{
    public class StoreNotificationCategoryModel
    {
        public string CategoryName { get; set; }
        public string Color { get; set; }
        public int Position { get; set; }

        public int ItemsCount {
            get
            {
                return Notifications == null ? 0 : Notifications.Count;
            }
        }
        public List<string> Notifications { get; set; }

    }
}
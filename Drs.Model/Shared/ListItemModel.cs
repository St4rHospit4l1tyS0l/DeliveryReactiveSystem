using System.Collections.Generic;

namespace Drs.Model.Shared
{
    public class ListItemModel
    {
        public long? IdKey { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public static class ListItemModelExtension
    {
        public static List<ListItemModel> InsertAllOption(this List<ListItemModel> lstValues, string defaultName = "Todos(as)")
        {
            lstValues.Insert(0, new ListItemModel
            {
                IdKey = -1,
                Key = "-1",
                Value = defaultName,
            });

            return lstValues;
        }
    }
}

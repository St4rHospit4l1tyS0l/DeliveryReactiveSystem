using System.Collections.Generic;

namespace Drs.Model.Shared
{
    public class OptionModel
    {
        public string StKey { get; set; }
        public string Name { get; set; }
    }

    public static class OptioModelExtension
    {
        public static List<OptionModel> InsertAllOption(this List<OptionModel> lstValues, string defaultName = "Todos(as)")
        {
            lstValues.Insert(0, new OptionModel
            {
                Name = defaultName,
                StKey = "-1"
            });

            return lstValues;
        }
    }

}

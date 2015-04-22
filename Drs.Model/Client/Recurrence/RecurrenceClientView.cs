using System.Linq;
using Drs.Model.Settings;

namespace Drs.Model.Client.Recurrence
{
    public class RecurrenceClientView
    {
        private static RecurrenceLevel _firstRecurrenceLevel;
        public string Color { get; set; }
        public string Icon { get; set; }
        public string Value { get; set; }

        public static RecurrenceLevel FirstRecurrenceLevel
        {
            get
            {
                return _firstRecurrenceLevel ??
                       (_firstRecurrenceLevel =
                           SettingsData.Recurrence.LstRecurrenceLevelTime.First(e => e.Value.Level == 0).Value);
            }
        }

        public RecurrenceClientView()
        {
            var unranked = FirstRecurrenceLevel;
            Value = unranked.Name;
            Color = unranked.Color;
            Icon = unranked.Icon;
        }
    }
}
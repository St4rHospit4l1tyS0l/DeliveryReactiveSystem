using System;
using System.Collections.Generic;
using System.Linq;
using Drs.Model.Settings;

namespace Drs.Model.Client.Recurrence
{
    public class RecurrenceClientModel
    {
        public int ClientId { get; set; }
        public List<String> LstName { get; set; }
        public List<decimal> LstValue { get; set; }

        public RecurrenceClientModel()
        {
            LstName = new List<string>();
            LstValue = new List<decimal>();
        }

        public List<RecurrenceClientView> ToListRecurrence()
        {
            return LstValue.Select((value, i) => CalculateRecurrence(value, i/2)).ToList();
        }

        private RecurrenceClientView CalculateRecurrence(decimal value, int type)
        {
            var lastLevel = RecurrenceClientView.FirstRecurrenceLevel;

            switch (type)
            {
                case 0:
                    foreach (var recurrenceLevel in SettingsData.Recurrence.LstRecurrenceLevelTime)
                    {
                        if (recurrenceLevel.Value.LevelValue <= (float) value)
                        {
                            lastLevel = recurrenceLevel.Value;
                            continue;
                        }
                        break;
                    }
                    return new RecurrenceClientView
                    {
                        Color = lastLevel.Color,
                        Icon = lastLevel.Icon,
                        Value = lastLevel.Name
                    };
                default:
                    foreach (var recurrenceLevel in SettingsData.Recurrence.LstRecurrenceLevelTotal)
                    {
                        if (recurrenceLevel.Value.LevelValue <= (float) value)
                        {
                            lastLevel = recurrenceLevel.Value;
                            continue;
                        }
                        break;
                    }
                    return new RecurrenceClientView
                    {
                        Color = lastLevel.Color,
                        Icon = lastLevel.Icon,
                        Value = lastLevel.Name
                    };
            }
        }
    }
}
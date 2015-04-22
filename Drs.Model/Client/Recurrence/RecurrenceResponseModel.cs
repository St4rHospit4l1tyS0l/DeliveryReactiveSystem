using System.Collections.Generic;

namespace Drs.Model.Client.Recurrence
{
    public class RecurrenceResponseModel
    {
        public List<RecurrenceClientModel> LstRecurrence { get; set; }

        public RecurrenceResponseModel()
        {
            LstRecurrence = new List<RecurrenceClientModel>();
        }
    }
}

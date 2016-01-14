using Newtonsoft.Json;

namespace CentralManagement.Models
{
    public class DailySalesModel
    {
        public string Date { get; set; }
        public double SubTotal { get; set; }
        public double Tax { get; set; }
        public double Total  { get; set; }
    }
}

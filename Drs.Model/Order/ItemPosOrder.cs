namespace Drs.Model.Order
{
    public class ItemPosOrder
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string CurPrice {
            get
            {
                return Price.ToString("C");
            }
        }
    }
}
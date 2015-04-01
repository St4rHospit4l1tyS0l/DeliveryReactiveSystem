namespace Drs.Model.Order
{
    public class ItemModel
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public bool IsIdSpecified { get; set; }
        public double Price { get; set; }
    }
}

namespace Drs.Model.Order
{
    public class InfoItemModel
    {
        public int LevelItem { get; set; }
        public string Name { get; set; }
        public long ParentId { get; set; }
        public decimal Price { get; set; }
        public long PosOrderItemId { get; set; }
    }
}
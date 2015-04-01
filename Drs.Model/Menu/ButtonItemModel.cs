namespace Drs.Model.Menu
{
    public class ButtonItemModel : IButtonItemModel
    {
        public string Color { get; set; }
        public string Image { get; set; }
        public int Position { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public dynamic DataInfo { get; set; }
    }
}
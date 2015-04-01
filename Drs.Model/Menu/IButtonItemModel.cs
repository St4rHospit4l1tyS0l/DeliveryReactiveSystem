namespace Drs.Model.Menu
{
    public interface IButtonItemModel
    {
        string Color { get; set; }
        string Image { get; set; }
        int Position { get; set; }
        string Title { get; set; }
        string Code { get; set; }
        string Description { get; set; }
        dynamic DataInfo { get; set; }

    }
}

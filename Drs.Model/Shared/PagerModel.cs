using Drs.Model.Settings;

namespace Drs.Model.Shared
{
    public class PagerModel
    {
        public PagerModel()
        {
            Size = SettingsData.Client.RowsSizeGrids;
        }
        public int Page { get; set; }
        public int Size { get; set; }
        public int Total { get; set; }

        public int SkipRow
        {
            get
            {
                return Page*Size;
            }
        }

        public int CalculatePages()
        {
            if (Total == 0)
                return 0;

            return (Total/Size) + 1;
        }
    }
}
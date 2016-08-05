using System.Windows;

namespace Drs.Ui.Ui.Track
{
    public partial class TrackOrderUc
    {
        public TrackOrderUc()
        {
            InitializeComponent();
            VwBoxTrackOrder.Height = SystemParameters.PrimaryScreenHeight;
            VwBoxTrackOrder.Width = SystemParameters.PrimaryScreenWidth;
        }

    }
}

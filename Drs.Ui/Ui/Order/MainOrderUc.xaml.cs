using System.Windows;

namespace Drs.Ui.Ui.Order
{
    /// <summary>
    /// Interaction logic for MainOrderUc.xaml
    /// </summary>
    public partial class MainOrderUc
    {
        public MainOrderUc()
        {
            InitializeComponent();
            VwBoxMainOrder.Height = SystemParameters.PrimaryScreenHeight;
            VwBoxMainOrder.Width = SystemParameters.PrimaryScreenWidth;

        }

    }
}

using System.Windows;
using Drs.Infrastructure.Ui;

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
            ScreenSizeResponsive.FixSize(MainGrid);
            ScreenSizeResponsive.FixClientAddressWidth(ClientControl);
            ScreenSizeResponsive.FixClientAddressWidth(AddressControl);
            //MainGrid.Height = 900;
            ScreenSizeResponsive.FixOrderDetailWidth(MainGrid.ColumnDefinitions);
        }
            
    }
}

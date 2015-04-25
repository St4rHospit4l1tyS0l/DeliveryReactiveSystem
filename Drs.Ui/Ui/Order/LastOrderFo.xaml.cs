using System.Windows;
using Drs.ViewModel.Shared;

namespace Drs.Ui.Ui.Order
{
    /// <summary>
    /// Interaction logic for LastOrderFo.xaml
    /// </summary>
    public partial class LastOrderFo 
    {
        public LastOrderFo()
        {
            InitializeComponent();
            IsOpenChanged += OnIsOpenChanged;
        }

        private void OnIsOpenChanged(object sender, RoutedEventArgs e)
        {
            var upsertVm = DataContext as IFlyoutBaseVm;
            if (upsertVm == null)
                return;

            upsertVm.IsOpenFinished = true;
        }

        //private void Storyboard_OnCompleted(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //  FirstName.Focus();
        //}
    }
}

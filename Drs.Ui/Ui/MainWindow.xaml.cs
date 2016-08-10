using System;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using System.Windows;
using Drs.Model.Constants;
using Drs.Model.Shared;
using MahApps.Metro;
using ReactiveUI;
using  MahApps.Metro.Controls.Dialogs;

namespace Drs.Ui.Ui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            MessageBus.Current.Listen<MessageBoxSettings>(SharedMessageConstants.MSG_SHOW_ERRQST)
                .Subscribe(x => RxApp.MainThreadScheduler.Schedule(_ => ShowMessageErrorOrQuestion(x)));

            MessageBus.Current.Listen<MessageBoxSettings>(SharedMessageConstants.MSG_SHOW_SUCCESS)
                .Subscribe(x => RxApp.MainThreadScheduler.Schedule(_ => ShowMessageSuccess(x)));
        }

        private async void ShowMessageSuccess(MessageBoxSettings settings)
        {
            await ShowMessage(settings, "Green");
        }

        private async void ShowMessageErrorOrQuestion(MessageBoxSettings settings)
        {
            await ShowMessage(settings, "Yellow");
        }

        private async Task ShowMessage(MessageBoxSettings settings, string sAccent)
        {
            settings.Settings.ColorScheme = MetroDialogColorScheme.Accented;
            var oldTheme = ThemeManager.DetectAppStyle(Application.Current);
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent(sAccent),
                ThemeManager.GetAppTheme("BaseLight"));
            var result = await this.ShowMessageAsync(settings.Title, settings.Message, settings.Style, settings.Settings);
            ThemeManager.ChangeAppStyle(Application.Current, oldTheme.Item2, ThemeManager.GetAppTheme("BaseLight"));
            if (settings.Callback != null)
                settings.Callback(result);
        }


        private void MainWindow_OnDeactivated(object sender, EventArgs e)
        {
            //var dataContext = (IShellContainerVm)DataContext;
            //Topmost = !dataContext.IsInOrder;
        }

        private void MainWindow_OnLostFocus(object sender, RoutedEventArgs e)
        {
            //var dataContext = (IShellContainerVm)DataContext;
            //Topmost = !dataContext.IsInOrder;
        }

    }
}

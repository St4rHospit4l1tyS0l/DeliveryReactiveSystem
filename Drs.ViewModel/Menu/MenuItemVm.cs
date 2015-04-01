using System;
using System.Reactive.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Drs.Infrastructure.Ui;
using Drs.Model.Menu;
using Drs.Model.UiView.Shared;
using Drs.ViewModel.Main;
using Drs.ViewModel.Shared;
using ReactiveUI;

namespace Drs.ViewModel.Menu
{
    public class MenuItemVm : UcViewModelBase, IMenuItemVm
    {
        private const string UriResource = "pack://application:,,,/Resources/Images/";

        public MenuItemVm(IButtonItemModel model, IShellContainerVm shellContainerVm)
            : base(shellContainerVm)
        {
            MenuItemBackgroundColor = new SolidColorBrush(model.Color.ToRgbColor());
            MenuItemBackgroundOverColor = new SolidColorBrush(model.Color.ToRgbLightColor(20));
            MenuItemBackgroundPressedColor = new SolidColorBrush(model.Color.ToRgbLightColor(-20));
            MenuItemLogo = new BitmapImage(new Uri(UriResource + model.Image));
            Title = model.Title;
            ExecuteCommand = ReactiveCommand.Create(Observable.Return(true));
            ExecuteCommand.Subscribe(_ => ShellContainerVm.ChangeCurrentView(model.Code.ToEnum(), true));
        }

        public IReactiveCommand<object> ExecuteCommand { get; private set; }

        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _title, value);
            }
        }

        private ImageSource _menuItemLogo;
        public ImageSource MenuItemLogo
        {
            get
            {
                return _menuItemLogo;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _menuItemLogo, value);
            }
        }



        private Brush _menuItemBackgroundColor;
        public Brush MenuItemBackgroundColor
        {
            get
            {
                return _menuItemBackgroundColor;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _menuItemBackgroundColor, value);
            }
        }


        private Brush _menuItemBackgroundOverColor;
        public Brush MenuItemBackgroundOverColor
        {
            get
            {
                return _menuItemBackgroundOverColor;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _menuItemBackgroundOverColor, value);
            }
        }


        private Brush _menuItemBackgroundPressedColor;
        public Brush MenuItemBackgroundPressedColor
        {
            get
            {
                return _menuItemBackgroundPressedColor;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _menuItemBackgroundPressedColor, value);
            }
        }
    }
}

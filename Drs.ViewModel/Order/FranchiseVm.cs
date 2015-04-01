using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Drs.Infrastructure.Extensions.Io;
using Drs.Infrastructure.Ui;
using Drs.Model.Menu;
using Drs.Model.Order;
using Drs.ViewModel.Main;
using Drs.ViewModel.Shared;
using ReactiveUI;

namespace Drs.ViewModel.Order
{
    public class FranchiseVm : UcViewModelBase, IFranchiseVm
    {
        private const string UriResource = "Logos\\";
        private readonly Action<FranchiseInfoModel> _onToggleButton;

        public FranchiseVm(IButtonItemModel model, IShellContainerVm shellContainerVm, Action<FranchiseInfoModel> onToggleButton)
            : base(shellContainerVm)
        {
            ItemBackgroundColor = new SolidColorBrush(model.Color.ToRgbColor());
            ItemBackgroundOverColor = new SolidColorBrush(model.Color.ToRgbLightColor(30));
            ItemBackgroundPressedColor = new SolidColorBrush(model.Color.ToRgbLightColor(-30));
            var uri = new Uri((UriResource + model.Image).AbsolutePathRelativeToEntryPointLocation());
            ItemLogo = new BitmapImage(uri);
            Title = model.Title;
            Products = model.Description;
            Code = model.Code;
            DataInfo = model.DataInfo;
            _onToggleButton = onToggleButton;
            //ExecuteCommand = ReactiveCommand.Create(Observable.Return(true));;
            //ExecuteCommand.Subscribe(_ => ShellContainerVm.ChangeCurrentView(model.Code.ToEnum(), true));
        }

        public dynamic DataInfo { get; set; }

        public string Code { get; set; }

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

        public String Products
        {
            get { return _products; }
            set
            {
                this.RaiseAndSetIfChanged(ref _products, value);
            }
        }

        private ImageSource _itemLogo;
        public ImageSource ItemLogo
        {
            get
            {
                return _itemLogo;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _itemLogo, value);
            }
        }



        private Brush _itemBackgroundColor;
        public Brush ItemBackgroundColor
        {
            get
            {
                return _itemBackgroundColor;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _itemBackgroundColor, value);
            }
        }


        private Brush _itemBackgroundOverColor;
        public Brush ItemBackgroundOverColor
        {
            get
            {
                return _itemBackgroundOverColor;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _itemBackgroundOverColor, value);
            }
        }


        private Brush _itemBackgroundPressedColor;
        private string _products;
        private bool _isChecked;

        public Brush ItemBackgroundPressedColor
        {
            get
            {
                return _itemBackgroundPressedColor;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _itemBackgroundPressedColor, value);
            }
        }

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                this.RaiseAndSetIfChanged(ref _isChecked, value);
                if (_onToggleButton != null && value)
                {
                    _onToggleButton(new FranchiseInfoModel{Code = Code, Title = Title, DataInfo = DataInfo});
                }
            }
        }
    }
}

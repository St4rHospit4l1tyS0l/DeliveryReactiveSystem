using System;
using System.Reactive.Linq;
using MahApps.Metro.Controls;
using ReactiveUI;

namespace Drs.ViewModel.Shared
{
    public abstract class FlyoutBaseVm : UcViewModelBase, IFlyoutBaseVm
    {
        private string _header;

        private bool _isOpen;

        private Position _position;

        protected FlyoutBaseVm()
        {
            CancelCommand = ReactiveCommand.Create(Observable.Return(true));
            CancelCommand.Subscribe(_ =>IsOpen = false);
        }

        public string Header
        {
            get
            {
                return _header;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _header, value);
            }
        }

        public Boolean IsOpen
        {
            get
            {
                return _isOpen;
            }
            set
            {
                IsOpenFinished = false;
                this.RaiseAndSetIfChanged(ref _isOpen, value);
            }
        }

        public Position Position
        {
            get
            {
                return _position;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _position, value);
            }
        }

        public IReactiveCommand<object> CancelCommand
        {
            get;
            private set;
        }

        public bool IsOpenFinished { get; set; }
    }
}
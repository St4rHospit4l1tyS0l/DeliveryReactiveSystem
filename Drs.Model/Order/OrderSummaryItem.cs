using System;
using System.Reactive;
using System.Windows;
using ReactiveUI;

namespace Drs.Model.Order
{
    public class OrderSummaryItem : ReactiveObject
    {
        private Visibility _hasToShow;
        private Visibility _isOk;
        private Visibility _isError;
        private Visibility _isSaveInProgress;
        private String _msgErr;
        private string _firstValue;
        private string _secondValue;
        private Visibility _isInProgress;

        public OrderSummaryItem()
        {
            ResetItem();
        }

        public void ResetItem()
        {
            MsgErr = String.Empty;
            FirstValue = String.Empty;
            SecondValue = String.Empty;
            HasToShow = Visibility.Collapsed;
            IsOk = Visibility.Hidden;
            IsError = Visibility.Collapsed;
            IsSaveInProgress = Visibility.Collapsed;
            IsInProgress = Visibility.Collapsed;
        }

        public Visibility HasToShow
        {
            get { return _hasToShow; }
            set { this.RaiseAndSetIfChanged(ref _hasToShow, value); }
        }
        
        public Visibility IsOk
        {
            get { return _isOk; }
            set { this.RaiseAndSetIfChanged(ref _isOk, value); }
        }
        
        public Visibility IsError
        {
            get { return _isError; }
            set { this.RaiseAndSetIfChanged(ref _isError, value); }
        }
        
        public Visibility IsSaveInProgress
        {
            get { return _isSaveInProgress; }
            set { this.RaiseAndSetIfChanged(ref _isSaveInProgress, value); }
        }

        public Visibility IsInProgress
        {
            get { return _isInProgress; }
            set { this.RaiseAndSetIfChanged(ref _isInProgress, value); }

        }

        public String MsgErr
        {
            get { return _msgErr; }
            set
            {
                this.RaiseAndSetIfChanged(ref _msgErr, value);
                if (String.IsNullOrEmpty(value) == false)
                    HasToShow = Visibility.Visible; 
            }
        }

        public IReactiveCommand<Unit> RetrySave { get; set; }

        public string FirstValue
        {
            get { return _firstValue; }
            set
            {
                this.RaiseAndSetIfChanged(ref _firstValue, value);
                HasToShow = String.IsNullOrEmpty(value) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public string SecondValue
        {
            get { return _secondValue; }
            set
            {
                this.RaiseAndSetIfChanged(ref _secondValue, value);
                if(String.IsNullOrEmpty(value) == false)
                    HasToShow = Visibility.Visible;
            }
        }
    }
}

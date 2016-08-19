using System;
using System.Windows;
using Drs.Model.Settings;
using Drs.ViewModel.Shared;
using Drs.ViewModel.Track;
using ReactiveUI;

namespace Drs.ViewModel.Status
{
    public class DailyOrderStatusVm : UcViewModelBase, IDailyOrderStatusVm
    {
        private IUcViewModel _backPrevious;
        private IUcViewModel _searchDaily;
        private IUcViewModel _ordersListTrack;
        private IOrderDetailVm _orderDetail;
        private Visibility _loadingVisibility;
        private Visibility _orderDetailVisibility;
        private Visibility _orderListVisibility;
        private string _eventsMsg;
        private string _errorMsg;
        private Visibility _errorVisibility;
        //private readonly IDictionary<int, IUcViewModel> _dicTabItems;

        public DailyOrderStatusVm(IBackPreviousVm backPreviousVm, ISearchDailyOrderVm searchDaily, IOrdersListVm ordersListTrack, IOrderDetailVm orderDetail)
        {

            BackPrevious = backPreviousVm;
            SearchDaily = searchDaily;
            OrdersListTrack = ordersListTrack;
            OrderDetail = orderDetail;
            
            //_dicTabItems = new Dictionary<int, IUcViewModel>
            //{
            //    {SharedConstants.Client.ORDER_TAB_PHONE, _searchTrack},
            //};
            //LstChildren.AddRange(_dicTabItems.Values);

            LstChildren.Add(_searchDaily);
            LstChildren.Add(_backPrevious);
            LstChildren.Add(_ordersListTrack);
            LstChildren.Add(_orderDetail);

            InitializeServices(searchDaily, ordersListTrack, orderDetail);
        }

        private void InitializeServices(ISearchDailyOrderVm searchDaily, IOrdersListVm ordersListTrack, IOrderDetailVm orderDetail)
        {
            searchDaily.DailySearch += ordersListTrack.OnDailySearchCommand;
            //searchDaily.ClientNameChanged += ordersListTrack.OnClientNameChanged;
            ordersListTrack.StatusChanged += OnStatusChanged;
            ordersListTrack.ShowDetail += orderDetail.OnShowDetail;
            orderDetail.StatusChanged += OnStatusChanged;

        }

        private void OnStatusChanged(int iStatus, string sMsg)
        {
            EventsMsg = sMsg;

            switch (iStatus)
            {
                case SettingsData.Constants.TrackConst.SEARCH_ORDERLIST_ON_PROGRESS:
                    OrderListVisibility = Visibility.Collapsed;
                    OrderDetailVisibility = Visibility.Collapsed;
                    LoadingVisibility = Visibility.Visible;
                    ErrorVisibility = Visibility.Collapsed;
                    break;
                case SettingsData.Constants.TrackConst.SEARCH_ERROR_PHONE:
                    OrderListVisibility = Visibility.Collapsed;
                    OrderDetailVisibility = Visibility.Collapsed;
                    LoadingVisibility = Visibility.Collapsed;
                    ErrorVisibility = Visibility.Visible;
                    ErrorMsg = sMsg;
                    break;
                case SettingsData.Constants.TrackConst.SEARCH_ORDERLIST_OK:
                    OrderListVisibility = Visibility.Visible;
                    OrderDetailVisibility = Visibility.Collapsed;
                    LoadingVisibility = Visibility.Collapsed;
                    ErrorVisibility = Visibility.Collapsed;
                    break;
                case SettingsData.Constants.TrackConst.SEARCH_ORDERLIST_READY:
                    OrderListVisibility = Visibility.Visible;
                    OrderDetailVisibility = Visibility.Collapsed;
                    LoadingVisibility = Visibility.Collapsed;
                    ErrorVisibility = Visibility.Collapsed;
                    break;

                case SettingsData.Constants.TrackConst.SEARCH_SHOWDETAIL_ON_PROGRESS:
                    OrderListVisibility = Visibility.Visible;
                    OrderDetailVisibility = Visibility.Collapsed;
                    LoadingVisibility = Visibility.Visible;
                    ErrorVisibility = Visibility.Collapsed;
                    break;

                case SettingsData.Constants.TrackConst.SEARCH_SHOWDETAIL_OK:
                    OrderListVisibility = Visibility.Visible;
                    OrderDetailVisibility = Visibility.Visible;
                    LoadingVisibility = Visibility.Collapsed;
                    ErrorVisibility = Visibility.Collapsed;
                    break;
                case SettingsData.Constants.TrackConst.SEARCH_SHOWDETAIL_ERROR:
                    OrderListVisibility = Visibility.Visible;
                    OrderDetailVisibility = Visibility.Collapsed;
                    LoadingVisibility = Visibility.Collapsed;
                    ErrorVisibility = Visibility.Visible;
                    ErrorMsg = sMsg;
                    break;
                default:
                    InitializeVisibility();
                    break;
            }    
        }


        public override bool Initialize(bool bForceToInit = false)
        {
            InitializeVisibility();
            return base.Initialize(true);
        }

        private void InitializeVisibility()
        {
            OrderListVisibility = Visibility.Collapsed;
            OrderDetailVisibility = Visibility.Collapsed;
            LoadingVisibility = Visibility.Collapsed;
            ErrorVisibility = Visibility.Collapsed;
            ErrorMsg = String.Empty;
            EventsMsg = String.Empty;
        }

        public Visibility LoadingVisibility
        {
            get { return _loadingVisibility; }
            set
            {
                this.RaiseAndSetIfChanged(ref _loadingVisibility, value);
            }
        }

        public Visibility OrderDetailVisibility
        {
            get { return _orderDetailVisibility; }
            set
            {
                this.RaiseAndSetIfChanged(ref _orderDetailVisibility, value);
            }
        }

        public Visibility OrderListVisibility
        {
            get { return _orderListVisibility; }
            set
            {
                this.RaiseAndSetIfChanged(ref _orderListVisibility, value);
            }
        }

        public Visibility ErrorVisibility
        {
            get { return _errorVisibility; }
            set
            {
                this.RaiseAndSetIfChanged(ref _errorVisibility, value);
            }
        }

        public string EventsMsg
        {
            get { return _eventsMsg; }
            set
            {
                this.RaiseAndSetIfChanged(ref _eventsMsg, value);
            }
        }

        public string ErrorMsg
        {
            get { return _errorMsg; }
            set
            {
                this.RaiseAndSetIfChanged(ref _errorMsg, value);
            }
        }


        public IUcViewModel SearchDaily
        {
            get { return _searchDaily; }
            set
            {
                this.RaiseAndSetIfChanged(ref _searchDaily, value);
            }
        }

        public IUcViewModel OrdersListTrack
        {
            get { return _ordersListTrack; }

            set { this.RaiseAndSetIfChanged(ref _ordersListTrack, value); }
        }

        public IOrderDetailVm OrderDetail
        {
            get { return _orderDetail; }
            set { this.RaiseAndSetIfChanged(ref _orderDetail, value); }
        }

        public IUcViewModel BackPrevious
        {
            get { return _backPrevious; }
            set
            {
                this.RaiseAndSetIfChanged(ref _backPrevious, value);
            }
        }
    }
}

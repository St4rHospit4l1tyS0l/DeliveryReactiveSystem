using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using Drs.Model.Address;
using Drs.Model.Constants;
using Drs.Model.Order;
using Drs.Model.Shared;
using Drs.Model.UiView.Shared;
using Drs.Service.Client;
using Drs.Service.Franchise;
using Drs.ViewModel.Main;
using Drs.ViewModel.Shared;
using ReactiveUI;

namespace Drs.ViewModel.Order
{
    public class MainOrderVm : UcViewModelBase, IMainOrderVm
    {
        private IUcViewModel _backPrevious;
        private IUcViewModel _searchNewPhone;
        private IUcViewModel _franchises;
        private IUcViewModel _clientsList;
        private IUcViewModel _addressList;
        private IUcViewModel _orderSummary;
        private IUcViewModel _orderPos;
        private IUcViewModel _sendOrder;

        private readonly IMainOrderService _orderService;
        private readonly IPosService _posService;
        private readonly ILastOrderFoVm _lastOrderFo;
        private int _selectedTab;
        private readonly IDictionary<int, IUcViewModel> _dicTabItems;

        public MainOrderVm(IBackPreviousVm backPreviousVm, ISearchNewPhoneVm searchNewPhone, IFranchiseContainerVm franchiseContainer,
            IClientsListVm clientsList, IAddressListVm addressList, IOrderSummaryVm orderSummary, IOrderPosVm orderPosVm, ISendOrderVm sendOrder,
            IMainOrderService orderService, IPosService posService, ILastOrderFoVm lastOrderFo)
        {

            BackPrevious = backPreviousVm;
            SearchNewPhone = searchNewPhone;
            Franchises = franchiseContainer;
            ClientsList = clientsList;
            AddressList = addressList;
            OrderPos = orderPosVm;
            SendOrder = sendOrder;

            clientsList.ValidateModel = orderService.ValidateModel;
            addressList.ValidateModel = orderService.ValidateModel;
            orderPosVm.ValidateModel = orderService.ValidateModel;
            sendOrder.ValidateModel = orderService.ValidateModel;

            _orderService = orderService;
            _posService = posService;
            _lastOrderFo = lastOrderFo;
            orderSummary.OrderService = orderService;
            OrderSummary = orderSummary;
            sendOrder.OrderService = orderService;

            clientsList.SetOrderModel(() => orderService.OrderModel);
            addressList.SetOrderModel(() => orderService.OrderModel);

            _dicTabItems = new Dictionary<int, IUcViewModel>
            {
                {SharedConstants.Client.ORDER_TAB_PHONE, _searchNewPhone},
                {SharedConstants.Client.ORDER_TAB_FRANCHISE, _franchises},
                {SharedConstants.Client.ORDER_TAB_CLIENTS, _clientsList},
                {SharedConstants.Client.ORDER_TAB_ORDER, _orderPos},
                {SharedConstants.Client.ORDER_TAB_DELIVERY, _sendOrder}
            };

            LstChildren.AddRange(_dicTabItems.Values);

            foreach (var wizard in LstChildren)
                wizard.NextStep += GoNextStep;

            LstChildren.Add(_backPrevious);
            LstChildren.Add(_addressList);
            LstChildren.Add(_orderSummary);

            SelectedTab = SharedConstants.Client.ORDER_TAB_PHONE;

            InitializeServices();
        }


        private void InitializeServices()
        {
            MessageBus.Current.Listen<ListItemModel>(SharedMessageConstants.ORDER_CLIENTPHONE).Subscribe(_orderService.ProcessPhone);
            MessageBus.Current.Listen<FranchiseInfoModel>(SharedMessageConstants.ORDER_FRANCHISE).Subscribe(_orderService.ProcessFranchise);
            MessageBus.Current.Listen<ClientInfoGrid>(SharedMessageConstants.ORDER_CLIENTINFO).Subscribe(_orderService.ProcessClient);
            MessageBus.Current.Listen<AddressInfoGrid>(SharedMessageConstants.ORDER_ADDRESSINFO).Subscribe(_orderService.ProcessAddress);
           
            var orderSummary = ((IOrderSummaryVm) OrderSummary);
            _orderService.PhoneChanged += orderSummary.OnPhoneChanged;
            _orderService.FranchiseChanged += orderSummary.OnFranchiseChanged;
            _orderService.FranchiseChanged += _posService.OnFranchiseChanged;

            var clientsList = ((IClientsListVm)ClientsList);
            _orderService.ClientChanged += clientsList.OnClientChanged;
            clientsList.ClientSelected += orderSummary.OnClientSelected;

            var addressList = ((IAddressListVm)AddressList);
            _orderService.AddressChanged += addressList.OnAddressChanged;
            addressList.ItemSelected += orderSummary.OnAddressSelected;

            MessageBus.Current.Listen<ListItemModel>(SharedMessageConstants.ORDER_CLIENTPHONE).Subscribe(clientsList.ProcessPhone);
            MessageBus.Current.Listen<ListItemModel>(SharedMessageConstants.ORDER_CLIENTPHONE).Subscribe(addressList.ProcessPhone);
            MessageBus.Current.Listen<ListItemModel>(SharedMessageConstants.ORDER_CLIENTPHONE).Subscribe(LastOrderFo.ProcessPhone);

            var sendOrder = ((ISendOrderVm)SendOrder);
            _orderService.PosOrderChanged += sendOrder.OnPosOrderChanged;  //Informa a la ventana para mostrar los valores obtenidos del POS
            _orderService.PosOrderChanged += orderSummary.OnPosOrderChanged;  //Informa al resumen del pedido
            _orderService.SendOrderToStoreStatusChanged += sendOrder.OnSendOrderToStoreStatusChanged;
            sendOrder.EndOrder += OnEndOrder;

            MessageBus.Current.Listen<PosCheck>(SharedMessageConstants.ORDER_SEND_POSORDER).Subscribe(x =>
            {
                _orderService.ProcessPosOrder(x);
                RxApp.MainThreadScheduler.Schedule(_ => { SelectedTab = SharedConstants.Client.ORDER_TAB_DELIVERY; });
            }); 

        }


        private void OnEndOrder()
        {
            ShellContainerVm.ChangeCurrentView(StatusScreen.ShMenu, true);
        }

        private void GoNextStep(int iNextItem)
        {
            SelectedTab = iNextItem;
        }

        protected override void OnShellContainerVmChange(IShellContainerVm value)
        {
            base.OnShellContainerVmChange(value);
            ShellContainerVm.AddOrUpdateFlyouts(LastOrderFo);
        }

        public override bool Initialize(bool bForceToInit = false)
        {
            var bResult = base.Initialize(true);
            _orderService.ResetAndCreateNewOrder();
            GoNextStep(SharedConstants.Client.ORDER_TAB_PHONE); 
            return bResult;
        }

        public IUcViewModel OrderPos
        {
            get { return _orderPos; }
            set
            {
                this.RaiseAndSetIfChanged(ref _orderPos, value);
            }
        }

        public IUcViewModel SendOrder
        {
            get { return _sendOrder; }
            set
            {
                this.RaiseAndSetIfChanged(ref _sendOrder, value);
            }
        }
        
        public IUcViewModel ClientsList
        {
            get { return _clientsList; }
            set
            {
                this.RaiseAndSetIfChanged(ref _clientsList, value);
            }
        }

        public IUcViewModel AddressList
        {
            get { return _addressList; }
            set
            {
                this.RaiseAndSetIfChanged(ref _addressList, value);
            }
        }

        public IUcViewModel Franchises
        {
            get { return _franchises; }
            set
            {
                this.RaiseAndSetIfChanged(ref _franchises, value);
            }
        }

        public IUcViewModel SearchNewPhone
        {
            get { return _searchNewPhone; }
            set
            {
                this.RaiseAndSetIfChanged(ref _searchNewPhone, value);
            }
        }

        public IUcViewModel BackPrevious
        {
            get { return _backPrevious; }
            set
            {
                this.RaiseAndSetIfChanged(ref _backPrevious, value);
            }
        }
        public IUcViewModel OrderSummary
        {
            get { return _orderSummary; }
            set
            {
                this.RaiseAndSetIfChanged(ref _orderSummary, value);
            }
        }


        public int SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedTab, value);

                IUcViewModel tabItem;
                if (_dicTabItems.TryGetValue(value, out tabItem) == false)
                    return;

                var response = tabItem.OnViewSelected(_selectedTab);
                if (response.IsSuccess || response.View == _selectedTab) 
                    return;
                
                MessageBus.Current.SendMessage(new MessageBoxSettings
                {
                    Message = response.Message,
                    Title = "Información faltante",
                }, SharedMessageConstants.MSG_SHOW_ERRQST);
                RxApp.MainThreadScheduler.Schedule(_ => { SelectedTab = response.View; });
            }
        }

        public ILastOrderFoVm LastOrderFo
        {
            get { return _lastOrderFo; }
        }
    }
}

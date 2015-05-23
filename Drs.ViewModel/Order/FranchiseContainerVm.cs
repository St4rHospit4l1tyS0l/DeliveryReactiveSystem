using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using Drs.Infrastructure.Extensions.Enumerables;
using Drs.Model.Constants;
using Drs.Model.Menu;
using Drs.Model.Order;
using Drs.Model.Shared;
using Drs.Service.ReactiveDelivery;
using Drs.Service.TransferDto;
using Drs.ViewModel.Main;
using Drs.ViewModel.Shared;
using ReactiveUI;

namespace Drs.ViewModel.Order
{
    public class FranchiseContainerVm : UcViewModelBase, IFranchiseContainerVm
    {
        public ReactiveList<IFranchiseVm> Items { get; set; }

        private readonly Func<IButtonItemModel, IShellContainerVm, Action<FranchiseInfoModel>, IFranchiseVm> _factoryFranchise;
        private readonly IReactiveDeliveryClient _client;
        private string _chosenFranchise;

        public FranchiseContainerVm(Func<IButtonItemModel, IShellContainerVm, Action<FranchiseInfoModel>, IFranchiseVm> factoryFranchise, IReactiveDeliveryClient client)
        {
            _factoryFranchise = factoryFranchise;
            _client = client;
            Items = new ReactiveList<IFranchiseVm>();

            MessageBus.Current.Listen<PropagateOrderModel>(SharedMessageConstants.PROPAGATE_LASTORDER_FRANCHISE).Subscribe(OnPropagate);
        }

        private void OnPropagate(PropagateOrderModel model)
        {
            var item = Items.FirstOrDefault(e => e.Code == model.PosCheck.FranchiseCode);
            if (item == null)
                return;
            
            SetNotCheckedButtons(new FranchiseInfoModel { Code = item.Code, Title = item.Title, DataInfo = item.DataInfo });

            if(model.Order != null)
                MessageBus.Current.SendMessage(model, SharedMessageConstants.PROPAGATE_LASTORDER_CLIENT);
        }

        //*
        public override bool Initialize(bool bForceToInit = false)
        {   
            if (base.Initialize(bForceToInit) == false)
                return false;

            _client.ExecutionProxy.ExecuteRequest<ResponseMessageData<ButtonItemModel>, ResponseMessageData<ButtonItemModel>>
                ( SharedConstants.Server.ORDER_HUB, SharedConstants.Server.LST_FRANCHISE_ORDER_HUB_METHOD, TransferDto.SameType)
                .ObserveOn(_client.ConcurrencyService.Dispatcher)
                .SubscribeOn(_client.ConcurrencyService.TaskPool)
                .Subscribe(OnMenuReady, OnMenuError);

            ChosenFranchise = String.Empty;

            return true;
        }

        private void OnMenuError(Exception ex)
        {
            MessageBox.Show(ex.Message);
        }

        private void OnMenuReady(IStale<ResponseMessageData<ButtonItemModel>> response)
        {
            if (response.IsStale)
            {
                OnMenuError(new Exception("Sin respuesta del servidor. Conexión no establecida."));
                return;
            }

            if (response.Data.IsSuccess == false)
            {
                OnMenuError(new Exception(response.Data.Message));
                return;
            }

            Items.Clear();
            foreach (var menuItemModel in response.Data.LstData){
                Items.Add(_factoryFranchise(menuItemModel, ShellContainerVm, SetNotCheckedButtons));
            }
        }

        private void SetNotCheckedButtons(FranchiseInfoModel franchiseInfo)
        {
            foreach (var franchiseVm in Items.Where(e => e.IsChecked && e.Code != franchiseInfo.Code)){
                franchiseVm.IsChecked = false;
            }
            ChosenFranchise = franchiseInfo.Title;

            MessageBus.Current.SendMessage(franchiseInfo, SharedMessageConstants.ORDER_FRANCHISE);
            MessageBus.Current.SendMessage(String.Empty, SharedMessageConstants.FLYOUT_LASTORDER_CLOSE);
            GoToNextStep(SharedConstants.Client.ORDER_TAB_CLIENTS);
        }

        public string ChosenFranchise
        {
            get { return _chosenFranchise; }
            set { this.RaiseAndSetIfChanged(ref _chosenFranchise, value); }
        }


    }
}

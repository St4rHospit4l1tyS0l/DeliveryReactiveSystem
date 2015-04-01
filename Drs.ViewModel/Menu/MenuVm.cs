using System;
using System.Reactive.Linq;
using System.Windows;
using Drs.Infrastructure.Extensions.Enumerables;
using Drs.Model.Account;
using Drs.Model.Constants;
using Drs.Model.Menu;
using Drs.Model.Shared;
using Drs.Service.ReactiveDelivery;
using Drs.Service.TransferDto;
using Drs.ViewModel.Main;
using Drs.ViewModel.Shared;
using ReactiveUI;

namespace Drs.ViewModel.Menu
{
    public class MenuVm : UcViewModelBase, IMenuVm
    {
        public ReactiveList<IMenuItemVm> MenuItems { get; set; }

        private readonly Func<IButtonItemModel, IShellContainerVm, IMenuItemVm> _factoryMenuItem;
        private readonly IReactiveDeliveryClient _client;

        public MenuVm(Func<IButtonItemModel, IShellContainerVm, IMenuItemVm> factoryMenuItem, IReactiveDeliveryClient client)
        {
            _factoryMenuItem = factoryMenuItem;
            _client = client;
            MenuItems = new ReactiveList<IMenuItemVm>();
        }

        public override bool Initialize(bool bForceToInit = false)
        {
            base.Initialize(bForceToInit);
            _client.ExecutionProxy.ExecuteRequest<String, String, ResponseMessageData<ButtonItemModel>, ResponseMessageData<ButtonItemModel>>
                (CurrentUserSettings.UserInfo.Username, TransferDto.SameType, SharedConstants.Server.ACCOUNT_HUB, SharedConstants.Server.MENU_INFO_ACCOUNT_HUB_METHOD, TransferDto.SameType)
                .ObserveOn(_client.ConcurrencyService.Dispatcher)
                .SubscribeOn(_client.ConcurrencyService.TaskPool)
                .Subscribe(OnMenuReady, OnMenuError);

            //Menu should no be set as Initialize because it must change always
            IsInitialize = false;
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

            if (response.Data.IsSuccess)
            {
                
                MenuItems.Clear();
                foreach (var menuItemModel in response.Data.LstData){
                    MenuItems.Add(_factoryMenuItem(menuItemModel, ShellContainerVm));
                }
            }
        }

            /*

        private void InitMenu()
        {

            MenuItems = new ReactiveList<IMenuItemVm>();
            for (var i = 0; i < 4; i++)
                MenuItems.Add(_factoryMenuItem(new ButtonItemModel{Color="#ff9900"}));
        }
             */ 
    }
}

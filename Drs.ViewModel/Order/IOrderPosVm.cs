using System;
using Drs.Model.Constants;
using Drs.Model.Shared;
using Drs.Model.Store;
using Drs.ViewModel.Shared;
using ReactiveUI;

namespace Drs.ViewModel.Order
{
    public interface IOrderPosVm : IUcViewModel
    {
        Func<ClientFlags.ValidateOrder, ResponseMessage> ValidateModel { get; set; }
        Action ReloadPosAction { get; set; }
        void OnChangeStore(ItemCatalog item, bool bIsLastStore);
        IReactiveList<StoreNotificationCategoryModel> LstNotificaionCategories { get; set; }
    }
}

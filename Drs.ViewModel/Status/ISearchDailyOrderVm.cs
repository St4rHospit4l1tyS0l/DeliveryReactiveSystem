using System;
using System.Reactive;
using Drs.Model.Shared;
using Drs.Model.Track;
using Drs.ViewModel.Shared;
using ReactiveUI;

namespace Drs.ViewModel.Status
{
    public interface ISearchDailyOrderVm : IUcViewModel
    {
        DateTime SearchDate { get; set; }
        ReactiveList<ItemCatalog> LstStores { get; set; }
        ItemCatalog AgentSelected { get; set; }
        ItemCatalog StoreSelected { get; set; }
        ReactiveList<ItemCatalog> LstAgents { get; set; }
        IReactiveCommand<Unit> SearchCommand { get; }

        event Action<DailySearchModel> DailySearch;
    }
}

using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Drs.Model.Catalog;
using Drs.Model.Shared;
using Drs.Model.Track;
using Drs.Service.ReactiveDelivery;
using Drs.ViewModel.Shared;
using ReactiveUI;

namespace Drs.ViewModel.Status
{
    public class SearchDailyOrderVm : UcViewModelBase, ISearchDailyOrderVm
    {
        private readonly IReactiveDeliveryClient _client;
        private DateTime _searchDate;
        private ItemCatalog _agentSelected;
        private ItemCatalog _storeSelected;
        private readonly DailySearchModel _dailySearchModel;

        public SearchDailyOrderVm(IReactiveDeliveryClient client)
        {
            _client = client;
            LstStores = new ReactiveList<ItemCatalog>(CatalogsClientModel.LstStores);
            StoreSelected = LstStores.Count > 0 ? LstStores[0] : null;
            LstAgents = new ReactiveList<ItemCatalog>(CatalogsClientModel.LstAgents);
            AgentSelected = LstAgents.Count > 0 ? LstAgents[0] : null;
            SearchCommand = ReactiveCommand.CreateAsyncTask(Observable.Return(true), _ => OnSearchCommand());
            _dailySearchModel = new DailySearchModel();
        }

        private async Task<Unit> OnSearchCommand()
        {
            _dailySearchModel.SearchDate = SearchDate;
            _dailySearchModel.StoreId = StoreSelected.Id;
            _dailySearchModel.AgentId = AgentSelected.Key;

            await Task.Run(() => OnDailySearch(_dailySearchModel));

            return new Unit();
        }

        public override bool Initialize(bool bForceToInit = false, string parameters = null)
        {
            SearchDate = DateTime.Today;    
            return base.Initialize(bForceToInit);
        }


        public DateTime SearchDate
        {
            get { return _searchDate; }
            set { this.RaiseAndSetIfChanged(ref _searchDate, value); }
        }

        public ReactiveList<ItemCatalog> LstStores { get; set; }

        public ReactiveList<ItemCatalog> LstAgents { get; set; }

        public ItemCatalog StoreSelected
        {
            get { return _storeSelected; }
            set { this.RaiseAndSetIfChanged(ref _storeSelected, value); }
        }
        public ItemCatalog AgentSelected
        {
            get { return _agentSelected; }
            set { this.RaiseAndSetIfChanged(ref _agentSelected, value); }
        }

        public IReactiveCommand<Unit> SearchCommand { get; private set; }

        public event Action<DailySearchModel> DailySearch;

        protected virtual void OnDailySearch(DailySearchModel obj)
        {
            var handler = DailySearch;
            if (handler != null) handler(obj);
        }
    }
}

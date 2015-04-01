using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using Drs.Infrastructure.Extensions.Enumerables;
using Drs.Model.Constants;
using Drs.Model.Shared;
using ReactiveUI;

namespace Drs.ViewModel.Shared
{
    public class AutoCompleteTextVm : UcViewModelBase, IAutoCompleteTextVm
    {
        private string _isDone;
        private string _search;
        private string _watermark;
        private bool _isFocused;

        public AutoCompleteTextVm()
        {
            IsDone = SharedConstants.Client.IS_FALSE;
            ListData = new ReactiveList<ListItemModel>();

            this.ObservableForProperty(x => x.Search)
                .Throttle(TimeSpan.FromMilliseconds(500))
                .Select(x => x.Value)
                .DistinctUntilChanged()
                .Where(search => !String.IsNullOrWhiteSpace(Search) && Search.Length > 1 && IsDone == SharedConstants.Client.IS_FALSE)
                .Do(_ => RxApp.MainThreadScheduler.Schedule(__ => ListData.Clear()))
                .Subscribe(x =>
                {
                    if (ExecuteSearch != null)
                        ExecuteSearch(x);
                });


            ExecuteEvent = ReactiveCommand.CreateAsyncTask(Observable.Return(true), async x =>
            {
                await Task.Run(() =>
                {
                    var itemModel = x as ListItemModel;
                    if (DoExecuteEvent != null)
                        DoExecuteEvent(itemModel);
                });
                return new Unit();
            });

        }

        public IReactiveCommand<Unit> ExecuteEvent { get; private set; }

        public string Watermark
        {
            get { return _watermark; }
            set { this.RaiseAndSetIfChanged(ref _watermark, value); }
        }

        public bool IsFocused
        {
            get { return _isFocused; }
            set { this.RaiseAndSetIfChanged(ref _isFocused, value); }
        }

        public string Search
        {
            get { return _search; }
            set { this.RaiseAndSetIfChanged(ref _search, value); }
        }

        public string IsDone
        {
            get { return _isDone; }
            set { this.RaiseAndSetIfChanged(ref _isDone, value); }
        }

        public ReactiveList<ListItemModel> ListData { get; set; }

        public event Action<string> ExecuteSearch;
        public event Action<ListItemModel> DoExecuteEvent;


        public void OnResultError(Exception ex)
        {
            MessageBox.Show(ex.Message);
        }

        public void OnResultReady(IStale<ResponseMessageData<ListItemModel>> response)
        {
            if (response.IsStale)
            {
                OnResultError(new Exception("Tiempo terminado"));
                return;
            }

            //TODO Revisar cuando on success no sea válido
            if (response.Data.IsSuccess == false)
            {
                OnResultError(new Exception(response.Data.Message));
                return;
            }

            RxApp.MainThreadScheduler.Schedule(_ =>
            {
                foreach (var itemModel in response.Data.LstData)
                {
                    var model = itemModel;
                    ListData.Add(model);
                }
            });
        }
    }

    public class AutoCompletePhoneVm : AutoCompleteTextVm, IAutoCompletePhoneVm
    {

    }
}

/*
                             //.Do(_ => RxApp.MainThreadScheduler.Schedule(__ => ListData.Clear()))
                            //.Select(search =>
                            //{
                            //    var lstPhones = _client.ExecutionProxy
                            //        .ExecuteRequest
                            //        <String, String, ResponseMessageData<ClientPhoneModel>, ResponseMessageData<ClientPhoneModel>>
                            //        (search.Value, TransferDto.SameType, SharedConstants.Server.CLIENT_HUB,
                            //            SharedConstants.Server.SEARCH_BY_PHONE_CLIENT_HUB_METHOD, TransferDto.SameType);


                //    return lstPhones;
                            //})
                            //.Switch()

 
 */

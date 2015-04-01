using System;
using System.Reactive;
using Drs.Infrastructure.Extensions.Enumerables;
using Drs.Model.Shared;
using ReactiveUI;

namespace Drs.ViewModel.Shared
{
    public interface IAutoCompleteTextVm : IUcViewModel
    {
        string Search { get; set; }
        string IsDone { get; set; }
        ReactiveList<ListItemModel> ListData { get; set; }
        
        event Action<string> ExecuteSearch;
        event Action<ListItemModel> DoExecuteEvent;
        
        IReactiveCommand<Unit> ExecuteEvent { get; }
        string Watermark { get; set; }
        bool IsFocused { get; set; }
        void OnResultReady(IStale<ResponseMessageData<ListItemModel>> response);
        void OnResultError(Exception ex);
    }

    public interface IAutoCompletePhoneVm : IAutoCompleteTextVm
    {
    }
}

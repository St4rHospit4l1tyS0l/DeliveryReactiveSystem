using System;
using System.Reactive;
using Drs.Model.Track;
using Drs.ViewModel.Shared;
using ReactiveUI;

namespace Drs.ViewModel.Track
{
    public interface IOrdersListVm : IUcViewModel
    {
        event Action<int, string> StatusChanged;

        event Action<long> ShowDetail;
        void OnPhoneChanged(string phone);        
        void OnClientNameChanged(int clientId);
        IReactiveList<TrackOrderDto> LstItems { get; set; }
        IReactiveCommand<Unit> CmdShowDetail { get; set; }
        IReactiveCommand<Unit> CmdCancelOrder { get; set; }
        IPagerVm Pager { get; set; }

    }
}

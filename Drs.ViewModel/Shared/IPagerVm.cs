using System;
using System.Reactive;
using Drs.Model.Shared;
using ReactiveUI;

namespace Drs.ViewModel.Shared
{
    public interface IPagerVm : IUcViewModel
    {
        int Pages { get; set; }
        int Page { get; set; }
        void Reset();
        PagerModel PagerModel { get; }
        int Size { get; set; }
        string TotalFound { get; set; }
        IReactiveCommand<Unit> CmdBackAll { get; set; }
        IReactiveCommand<Unit> CmdBack { get; set; }
        IReactiveCommand<Unit> CmdForward { get; set; }
        IReactiveCommand<Unit> CmdForwardAll { get; set; }
        
        event Action PagerChanged;
        void SetPager(PagerModel pager);
    }
}

using System;
using Drs.Model.Constants;
using ReactiveUI;

namespace Drs.ViewModel.Shared
{
    public class MsgWndVm : UcViewModelBase, IMsgWndVm
    {
        private string _msgInfo;

        public string MsgInfo
        {
            get { return _msgInfo; }
            set { this.RaiseAndSetIfChanged(ref _msgInfo, value); }
        }

        public MsgWndVm()
        {
            MessageBus.Current.Listen<string>(SharedMessageConstants.ACCOUNT_ERROR_CHECK)
                .Subscribe(x =>
                {
                    MsgInfo = x;
                });
        }
    }
}

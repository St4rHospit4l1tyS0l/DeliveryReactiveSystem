using System;
using System.Security.Policy;
using Drs.Model.Constants;
using Drs.Repository.Log;
using Drs.ViewModel.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReactiveUI;

namespace Drs.ViewModel.External
{
    public class BrowserVm : UcViewModelBase, IBrowserVm
    {
        private IUcViewModel _backPrevious;
        private string _title;

        public BrowserVm(IBackPreviousVm backPreviousVm)
        {

            BackPrevious = backPreviousVm;
            LstChildren.Add(_backPrevious);
        }

        public string UrlBrowser { get; set; }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                this.RaiseAndSetIfChanged(ref _title, value);
            }
        }

        public Action<string> RefreshBrowser { get; set; }

        public IUcViewModel BackPrevious
        {
            get { return _backPrevious; }
            set
            {
                this.RaiseAndSetIfChanged(ref _backPrevious, value);
            }
        }

        public override bool Initialize(bool bForceToInit = false, string parameters = null)
        {
            if (!string.IsNullOrWhiteSpace(parameters))
            {
                try
                {
                    dynamic parms = JObject.Parse(parameters);
                    Title = parms.Title;
                    UrlBrowser = parms.Url;
                }
                catch (Exception ex)
                {
                    SharedLogger.LogError(ex);
                }
            }
            return base.Initialize(true);
        }

    }
}

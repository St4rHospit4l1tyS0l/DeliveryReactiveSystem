using System;
using System.Deployment.Application;
using System.Reactive.Linq;
using System.Reflection;
using ReactiveUI;

namespace Drs.ViewModel.Shared
{
    public class SplashVm : ReactiveObject
    {
        private int _progress = -1;
        private string _productVersion;

        public SplashVm(int miliSecondsToInit)
        {
            ProductVersion = String.Format("Versión: " + CurrentVersion);

            ShowProgress = ReactiveCommand.CreateAsyncObservable(_ =>
            Observable.Timer(TimeSpan.Zero, TimeSpan.FromMilliseconds(miliSecondsToInit/100.0))
                .Take(miliSecondsToInit/10)
                .Scan(0, (acc, x) => 
                    acc + 1));

            ShowProgress.Subscribe(x =>
            {   
                Progress = x;
            });
        }

        public IReactiveCommand<int> ShowProgress { get; set; }

        public string CurrentVersion
        {
            get
            {
                return ApplicationDeployment.IsNetworkDeployed
                       ? ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString()
                       : Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }  

        public string ProductVersion
        {
            get { return _productVersion; }
            set { this.RaiseAndSetIfChanged(ref _productVersion, value); }
        }

        public int Progress
        {
            get { return _progress; }
            set { this.RaiseAndSetIfChanged(ref _progress, value); }
        }
    }
}

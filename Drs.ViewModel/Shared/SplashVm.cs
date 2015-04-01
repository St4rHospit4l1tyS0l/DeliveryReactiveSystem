using System;
using System.Reactive.Linq;
using ReactiveUI;

namespace Drs.ViewModel.Shared
{
    public class SplashVm : ReactiveObject
    {
        private int _progress = -1;

        public IReactiveCommand<int> ShowProgress { get; set; }
        

        public SplashVm(int miliSecondsToInit)
        {
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


        public int Progress
        {
            get { return _progress; }
            set { this.RaiseAndSetIfChanged(ref _progress, value); }
        }
    }
}

using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using ReactiveUI;

namespace Drs.Ui
{
    public class TestVm : ReactiveObject
    {
        public IReactiveCommand<Unit> GenerateCommand { get; set; }

        private string _username;
        public String Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                this.RaiseAndSetIfChanged(ref _username, value);
            }
        }

        public TestVm()
        {
            try
            {
                var canGenerate = this.WhenAny(vm => vm.GenerateCommand, x => true);
                GenerateCommand = ReactiveCommand.CreateAsyncTask(canGenerate, x =>
                {
                    return DoSomething();
                });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public async Task<Unit> DoSomething()
        {
            await Task.Run(() =>
            {
                Observable.Range(0, 20).Select(
                    x =>
                    {
                        Thread.Sleep(200);
                        return x;
                    }).Subscribe(x => Console.Write(true));
            });
            return new Unit();
        }
    }
}
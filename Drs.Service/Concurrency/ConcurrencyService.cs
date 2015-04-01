using System.Reactive.Concurrency;

namespace Drs.Service.Concurrency
{
    public sealed class ConcurrencyService : IConcurrencyService
    {
        public IScheduler Dispatcher
        {
            get { return DispatcherScheduler.Current; }
        }

        public IScheduler TaskPool
        {
            get { return ThreadPoolScheduler.Instance; }
        }
    }
}
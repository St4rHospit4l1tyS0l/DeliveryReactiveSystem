using System.Reactive.Concurrency;

namespace Drs.Service.Concurrency
{
    public interface IConcurrencyService
    {
         IScheduler Dispatcher { get; }
         IScheduler TaskPool { get; }
 
    }
}
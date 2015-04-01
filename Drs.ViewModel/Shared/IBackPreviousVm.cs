using ReactiveUI;

namespace Drs.ViewModel.Shared
{
    public interface IBackPreviousVm : IUcViewModel
    {
        IReactiveCommand<object> BackCommand { get; }
    }
}

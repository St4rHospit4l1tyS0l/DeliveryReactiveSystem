using System;

namespace Drs.ViewModel.Shared
{
    public interface IWizItem
    {
        event Action<int> NextStep;
        void GoToNextStep(int iNextStep);
    }
}
using System;
using System.Collections.Generic;
using Drs.Model.Shared;
using Drs.ViewModel.Main;
using ReactiveUI;

namespace Drs.ViewModel.Shared
{
    public class UcViewModelBase : ReactiveObject, IUcViewModel
    {
        private IShellContainerVm _shellContainerVm;
        protected List<IUcViewModel> LstChildren;
        //protected ICurrentUserSettings CurrentUserSettingsLogin;

        public UcViewModelBase()
        {
            LstChildren = new List<IUcViewModel>();
        }

        protected UcViewModelBase(IShellContainerVm shellContainerVm)
            :this()
        {
            ShellContainerVm = shellContainerVm;
        }

        public bool IsInitialize { get; set; }

        public IShellContainerVm ShellContainerVm
        {
            get { return _shellContainerVm; }
            set
            {
                _shellContainerVm = value;
                OnShellContainerVmChange(_shellContainerVm);
            }
        }

        protected virtual void OnShellContainerVmChange(IShellContainerVm value)
        {
            foreach (var ucViewModel in LstChildren)
            {
                ucViewModel.ShellContainerVm = value;
            }
        }

        public virtual bool Initialize(bool bForceToInit = false)
        {
            if (IsInitialize && !bForceToInit)
                return false;

            foreach (var ucViewModel in LstChildren)
            {
                ucViewModel.Initialize(bForceToInit);
            }

            IsInitialize = true;
            return IsInitialize;
        }

        public virtual ResponseMessage OnViewSelected(int iSelectedTab)
        {
            return new ResponseMessage{IsSuccess = true};
        }


        public event Action<int> NextStep;
        public void GoToNextStep(int iNextStep)
        {
            if (NextStep != null)
                NextStep(iNextStep);
        }
    }
}

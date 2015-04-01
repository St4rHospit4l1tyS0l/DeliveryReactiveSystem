using System;
using Drs.Model.Constants;
using Drs.Model.Shared;
using Drs.ViewModel.Shared;

namespace Drs.ViewModel.Order
{
    public interface IOrderPosVm : IUcViewModel
    {
        Func<ClientFlags.ValidateOrder, ResponseMessage> ValidateModel { get; set; }
    }
}

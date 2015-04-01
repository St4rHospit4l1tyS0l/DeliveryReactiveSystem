using System;
using System.Windows;
using Drs.Model.Track;
using Drs.ViewModel.Shared;

namespace Drs.ViewModel.Track
{
    public interface IOrderDetailVm : IUcViewModel
    {
        void OnShowDetail(long orderToStoreId);
        
        event Action<int, string> StatusChanged;
        TrackOrderDetailDto OrderDetail { get; set; }
        Visibility VisiblityStoreErrMsg { get; set; }
    }
}

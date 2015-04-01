using System;
using System.Windows;
using Drs.ViewModel.Shared;

namespace Drs.ViewModel.Track
{
    public interface ITrackOrderVm : IUcViewModel
    {
        IUcViewModel SearchTrack { get; set; }
        IUcViewModel BackPrevious { get; set; }
        IUcViewModel OrdersListTrack { get; set; }
        IOrderDetailVm OrderDetail { get; set; }
        Visibility LoadingVisibility { get; set; }
        Visibility OrderDetailVisibility { get; set; }
        Visibility OrderListVisibility { get; set; }
        Visibility ErrorVisibility { get; set; }
        String EventsMsg { get; set; }
        String ErrorMsg { get; set; }
        
    }
}
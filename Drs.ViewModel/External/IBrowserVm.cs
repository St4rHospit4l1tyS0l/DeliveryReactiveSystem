using System;
using Drs.ViewModel.Shared;

namespace Drs.ViewModel.External
{
    public interface IBrowserVm : IUcViewModel
    {
        IUcViewModel BackPrevious { get; set; }
        string Title { get; set; }
        string UrlBrowser { get; set; }
    }
}
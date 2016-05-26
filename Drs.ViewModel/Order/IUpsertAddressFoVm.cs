using System.Reactive;
using System.Windows;
using Drs.Model.Address;
using Drs.Model.Order;
using Drs.Model.Shared;
using Drs.ViewModel.Shared;
using ReactiveUI;

namespace Drs.ViewModel.Order
{
    public interface IUpsertAddressFoVm : IFlyoutBaseVm
    {
        string ZipCode { get; set; }
        int? ZipCodeId { get; set; }
        IReactiveCommand<Unit> UpsertCommand { get; }
        IAutoCompleteTextVm ZipCodeSearchVm{ get; }
        bool IsSearchByCode { get; set; }
        bool IsSearchByMap { get; set; }
        bool IsSearchByWaterfall { get; set; }
        bool IsZipCodeSearchEnabled { get; set; }
        Visibility VisibilityMap { get; set; }
        Visibility VisibilityManual { get; set; }
        string ErrorSearch { get; set; }
        Visibility ErrorSearchVisibility { get; set; }
        string ErrorUpsert { get; set; }
        Visibility ErrorUpsertVisibility { get; set; }
        IReactiveList<ListItemModel> Countries { get; set; }
        IReactiveList<ListItemModel> RegionsA { get; set; }
        IReactiveList<ListItemModel> RegionsB { get; set; }
        IReactiveList<ListItemModel> RegionsC { get; set; }
        IReactiveList<ListItemModel> RegionsD { get; set; }
        ListItemModel CountrySel { get; set; }
        ListItemModel RegionArSel { get; set; }
        ListItemModel RegionBrSel { get; set; }
        ListItemModel RegionCrSel { get; set; }
        ListItemModel RegionDrSel { get; set; }
        FranchiseInfoModel Franchise { get; set; }  
        string MainAddress { get; set; }
        string Reference { get; set; }
        string NumExt { get; set; }
        void Fill(AddressInfoGrid clInfo, FranchiseInfoModel franchise);
        void Clean(FranchiseInfoModel franchise);
    }
}

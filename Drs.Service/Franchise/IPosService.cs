using Drs.Model.Order;

namespace Drs.Service.Franchise
{
    public interface IPosService
    {
        void OnFranchiseChanged(FranchiseInfoModel obj);
        //bool ValidatePrices(PosCheck posCheck);
    }
}

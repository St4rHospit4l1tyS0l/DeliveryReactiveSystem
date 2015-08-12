using Drs.Model.Shared;

namespace Drs.Model.Order
{
    public class PropagateOrderModel
    {
        public OrderInfoModel Order { get; set; }
        public PosCheck PosCheck { get; set; }
        public OptionModel Franchise { get; set; }
        public bool HasEdit { get; set; }
    }
}

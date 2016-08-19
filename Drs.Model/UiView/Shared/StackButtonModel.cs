using System;
using Drs.Model.Order;
using ReactiveUI;

namespace Drs.Model.UiView.Shared
{
    public class StackButtonModel : ReactiveObject
    {

        public String Content {
            get
            {
                return String.Format("{0}{1}{2}{3}{4} | $ {5:#,##0.00}", _order.OrderDate.ToString("f"), Environment.NewLine, 
                    _order.ClientName, Environment.NewLine, _order.StoreName, _order.Total);
            }
        }
        public long OrderToStoreId
        {
            get
            {
                return _order.OrderToStoreId;
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set { this.RaiseAndSetIfChanged(ref _isSelected, value); }
        }

        private readonly LastOrderInfoModel _order;
        private bool _isSelected;

        public StackButtonModel(LastOrderInfoModel order)
        {
            _order = order;
        }
    }
}

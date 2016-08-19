using System;

namespace Drs.Model.Order
{
    public class ItemPosOrder
    {
        private string _name;
        public int ItemId { get; set; }

        public string Name
        {
            get{ return _name;  }
            set { _name = value; }
        }

        public string NameEx
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_name) || Level == 0)
                    return _name;


                return _name.Insert(0, String.Empty.PadLeft(Level * Constants.SharedConstants.LEVEL_PAD_ITEM, ' '));
            }
        }

        public decimal Price { get; set; }
        public string CurPrice {
            get
            {
                return Price.ToString("C");
            }
        }

        public int Level { get; set; }
        public long CheckItemId { get; set; }
    }
}
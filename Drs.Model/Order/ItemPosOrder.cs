using System;

namespace Drs.Model.Order
{
    public class ItemPosOrder
    {
        private string _name;
        public int ItemId { get; set; }

        public string Name
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_name) || Level == 0)
                    return _name;


                return _name.Insert(0, String.Empty.PadLeft((int)Level * 4, '.'));
            }
            set { _name = value; }
        }

        public decimal Price { get; set; }
        public string CurPrice {
            get
            {
                return Price.ToString("C");
            }
        }

        public int Level { get; set; }
    }
}
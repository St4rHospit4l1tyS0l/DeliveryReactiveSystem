using System;

namespace Drs.Model.Constants
{
    public class ClientFlags
    {
        [Flags]
        public enum ValidateOrder
        {
            Phone = 0x01,
            Franchise = 0x02,
            Client = 0x04,
            Address = 0x08,
            Order = 0x10,
            OrderSaved = 0x20
        }
    }
}

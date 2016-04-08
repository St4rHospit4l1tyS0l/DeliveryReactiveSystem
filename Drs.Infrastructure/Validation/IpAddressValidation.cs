using System;
using System.Linq;

namespace Drs.Infrastructure.Validation
{
    public class IpAddressValidation
    {
        public bool  CheckIpValid(String strIp)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(strIp))
                    return false;

                var arrOctets = strIp.Split('.');
                if (arrOctets.Length != 4)
                    return false;

                //Check each substring checking that parses to byte
                byte obyte;
                return arrOctets.All(strOctet => byte.TryParse(strOctet, out obyte));
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

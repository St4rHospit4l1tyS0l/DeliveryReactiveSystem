using System.ServiceModel;
using System.ServiceModel.Description;

namespace Drs.Service.Transport
{
    public static class WcfExt
    {
        public static void SetMtomEncodingAndSize(ServiceEndpoint endpoint)
        {
            var basicBinding = ((BasicHttpBinding)endpoint.Binding);
            basicBinding.MessageEncoding = WSMessageEncoding.Mtom;
            basicBinding.MaxReceivedMessageSize = 100000000;
            basicBinding.MaxBufferSize = 100000000;
        }

    }
}

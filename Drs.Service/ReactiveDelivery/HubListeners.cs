using System;
using System.Collections.Generic;
using Drs.Model.Constants;
using Drs.Model.Shared;
using Microsoft.AspNet.SignalR.Client;

namespace Drs.Service.ReactiveDelivery
{
    public class HubListeners
    {
        public event Action<ResponseMessage> SendToStoreEventChanged;

        protected virtual void OnSendToStoreEventChanged(ResponseMessage status)
        {
            var handler = SendToStoreEventChanged;
            if (handler != null) handler(status);
        }

        public void InitializeListeners(IDictionary<string, IHubProxy> dicHubProxies)
        {
            foreach (var hubProxy in dicHubProxies)
            {
                switch (hubProxy.Key)
                {
                    case SharedConstants.Server.STORE_HUB:
                        {
                            hubProxy.Value.On("OnSendToStoreEventChange", (ResponseMessage response) => OnSendToStoreEventChanged(response));
                            break;
                        }
                }
            }
        }
    }
}
using System;
using Drs.Model.Settings;
using Drs.Service.Transport;

namespace Drs.Service.ServiceClient
{
    public class ExecuteServiceClient : ServiceClientBase, IExecuteServiceClient
    {
        public ExecuteServiceClient(IConnectionProvider connectionProvider) : base(connectionProvider)
        {
        }

        public IObservable<TRes> ExecuteRequest<TRes>(string hub, string methodName, params object[] request)
        {
            return RequestUponConnection(connection =>
            {
                var result = ExecuteForConnection<TRes>(connection.DicHubProxy, hub, methodName, request);
                return result;
            }, TimeSpan.FromSeconds(SettingsData.Client.SencondsToWaitForResponse));
        } 
    }
}

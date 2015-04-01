using System;

namespace Drs.Service.ServiceClient
{
    public interface IExecuteServiceClient
    {
        IObservable<TRes> ExecuteRequest<TRes>(string hub, string methodName, params object[] request);
    }
}
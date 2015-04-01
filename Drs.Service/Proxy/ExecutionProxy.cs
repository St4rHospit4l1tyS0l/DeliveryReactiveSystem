using System;
using System.Reactive.Linq;
using Drs.Infrastructure.Extensions.Enumerables;
using Drs.Model.Settings;
using Drs.Service.Concurrency;
using Drs.Service.ServiceClient;

namespace Drs.Service.Proxy
{
    public class ExecutionProxy : IExecutionProxy
    {
        private readonly IExecuteServiceClient _executeServiceClient;
        private readonly IConcurrencyService _concurrencyService;

        public ExecutionProxy(IExecuteServiceClient executeServiceClient, IConcurrencyService concurrencyService)
        {
            _executeServiceClient = executeServiceClient;
            _concurrencyService = concurrencyService;
        }

        public IObservable<IStale<TRes>> ExecuteRequest<TReq, TReqDto, TResDto, TRes>
            (TReq preRequest, Func<TReq, TReqDto> funcRequest, string hub, string method, Func<TResDto, TRes> funcResponse)
        {
            var request = funcRequest(preRequest);
            return _executeServiceClient.ExecuteRequest<TResDto>(hub, method, request)
                .Select(funcResponse)
                .DetectStale(TimeSpan.FromSeconds(SettingsData.Client.SencondsToDetectStale), _concurrencyService.TaskPool);
        }

        public IObservable<IStale<TRes>> ExecuteRequest<TResDto, TRes>(string hub, string method, Func<TResDto, TRes> funcResponse)
        {
            return _executeServiceClient.ExecuteRequest<TResDto>(hub, method)
                .Select(funcResponse)
                .DetectStale(TimeSpan.FromSeconds(60), _concurrencyService.TaskPool);
        }
    }
}
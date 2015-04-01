using System;
using Drs.Infrastructure.Extensions.Enumerables;

namespace Drs.Service.Proxy
{
    public interface IExecutionProxy
    {
        IObservable<IStale<TRes>> ExecuteRequest<TReq, TReqDto, TResDto, TRes>
            (TReq preRequest, Func<TReq, TReqDto> funcRequest, string hub, string method, Func<TResDto, TRes> funcResponse);

        IObservable<IStale<TRes>> ExecuteRequest<TResDto, TRes>(string hub, string method,
            Func<TResDto, TRes> funcResponse);

    }
}
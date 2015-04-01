namespace Drs.Infrastructure.Extensions.Enumerables
{
    public interface IStale<out T>
    {
        bool IsStale { get; }
        T Data { get; }
    }
}
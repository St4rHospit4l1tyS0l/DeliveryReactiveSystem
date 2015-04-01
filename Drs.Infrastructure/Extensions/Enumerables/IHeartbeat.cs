namespace Drs.Infrastructure.Extensions.Enumerables
{
    public interface IHeartbeat<out T>
    {
        bool IsHeartbeat { get; }
        T Update { get; }
    }
}
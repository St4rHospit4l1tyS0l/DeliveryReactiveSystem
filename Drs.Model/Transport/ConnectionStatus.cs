namespace Drs.Model.Transport
{
    public enum ConnectionStatus
    {
        Connecting,
        Connected,
        ConnectionSlow,
        Reconnecting,
        Reconnected,
        Closed,
        Uninitialized
    }
}
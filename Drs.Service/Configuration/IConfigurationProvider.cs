namespace Drs.Service.Configuration
{
    public interface IConfigurationProvider
    {
        string[] Servers { get; }
    }
}
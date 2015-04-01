using System.Configuration;
using Drs.Service.Configuration;

namespace Drs.Ui.Configuration
{
    class ConfigurationProvider : IConfigurationProvider
    {
        public string[] Servers
        {
            get
            {
                var servers = ConfigurationManager.AppSettings["servers"];
                if (string.IsNullOrEmpty(servers))
                {
                    throw new ConfigurationErrorsException("La llave para definir los servidores no está definida.");
                }

                return servers.Split(';');
            }
        }
    }
}
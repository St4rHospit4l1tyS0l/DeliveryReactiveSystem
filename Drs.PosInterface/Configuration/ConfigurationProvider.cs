using System.Configuration;
using Drs.Service.Configuration;

namespace Drs.PosInterface.Configuration
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        public string[] Servers
        {
            get
            {
                const string servers = "http://localhost:41956/";
                if (string.IsNullOrEmpty(servers)){
                    throw new ConfigurationErrorsException("La llave para definir los servidores no está definida.");
                }

                return servers.Split(';');
            }
        }
    }
}
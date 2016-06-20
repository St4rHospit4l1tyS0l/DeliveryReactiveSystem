using System;
using System.Configuration;
using ConnectCallCenter;
using log4net;
using Microsoft.Owin.Hosting;

namespace HostConnectCallCenter
{
    public class Program
    {
        static void Main()
        {
            try
            {
                var ipPort = ConfigurationManager.AppSettings["IpPortHost"];

                Console.WriteLine("Server try to init at " + ipPort);
                LogManager.GetLogger("INFO").Info("Server try to init at " + ipPort);

                AppInit.Start();

                using (WebApp.Start<Startup>(ipPort))
                {
                    Console.WriteLine("Server running at " + ipPort);
                    Console.ReadLine();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }
}

using System;
using ConnectCallCenter;
using Microsoft.Owin.Hosting;

namespace HostConnectCallCenter
{
    public class Program
    {
        static void Main()
        {
            try
            {
                Console.WriteLine("Server try to init at http://localhost:8080/");

                AppInit.Start();

                using (WebApp.Start<Startup>("http://localhost:8080/"))
                {
                    Console.WriteLine("Server running at http://localhost:8080/");
                    Console.ReadLine();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

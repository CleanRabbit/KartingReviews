using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

using Summers.Wyvern.Server;


namespace Summers.Wyvern.DesktopHost
{
	class DesktopHost
    {
        static void Main()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true);
            var config = builder.Build();

            var host = new Host(config);
            Task.Run(()=>host.Start());

            Console.ReadLine();
            Task.Run(()=>host.ShutDown());
        }
    }
}

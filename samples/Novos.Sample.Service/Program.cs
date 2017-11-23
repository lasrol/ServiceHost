using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Novos.ServiceHost;

namespace Novos.Sample.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new ServiceHostBuilder()
                .AddConfiguration(o => o.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                )
                .AddLogging(o => o.AddConsole())
                .SetStartup<Startup>()
                .Build();


            host.Run();
        }
    }
}

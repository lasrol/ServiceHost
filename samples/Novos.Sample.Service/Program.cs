using System;
using System.IO;
using Autofac;
using Autofac.Core;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
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
                .SetStartup<Startup>()
                .Build();


            host.Run();
        }
    }
}

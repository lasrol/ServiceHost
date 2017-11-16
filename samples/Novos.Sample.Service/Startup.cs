using System;
using Microsoft.Extensions.DependencyInjection;
using Novos.ServiceHost;

namespace Novos.Sample.Service
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection containerBuilder)
        {
            
        }

        public void Run(ServiceContext serviceContext)
        {
            Console.WriteLine("Running...");
        }
    }
}

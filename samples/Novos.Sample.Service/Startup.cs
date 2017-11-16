using System;
using Microsoft.Extensions.Configuration;
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
            var config = serviceContext.Services.GetRequiredService<IConfiguration>();

            try
            {
                throw new Exception("Testing");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            //Console.WriteLine("Running...");
        }
    }
}

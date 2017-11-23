using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Novos.ServiceHost;
using Microsoft.Extensions.Logging;

namespace Novos.Sample.Service
{
    public class Startup : IServiceStartup
    {
        private readonly ILogger<Startup> _logger;

        public Startup(ILogger<Startup> logger)
        {
            _logger = logger;
        }
        public void ConfigureServices(IServiceCollection containerBuilder)
        {
            
        }

        public void ConfigurePipeline(IServiceAppBuilder app)
        {
            _logger.LogInformation("Sample logging");

            app.Run((context) =>
            {
                _logger.LogInformation("Using inline task");
                return Task.CompletedTask;
            });

            app.UseTask<RunTask>();

            app.UseTask(typeof(RunTask));

            app.Run((context) =>
            {
                _logger.LogInformation("Using second inline task");
                return Task.CompletedTask;
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Novos.ServiceHost
{
    public class ServiceHost
    {
        private readonly ServiceContext _context;
        private readonly IServiceStartup _startup;
        private ILogger<ServiceHost> _logger;

        public ServiceHost(ServiceContext serviceProvider)
        {
            _context = serviceProvider;
            _startup = _context.Services.GetRequiredService<IServiceStartup>();
        }

        public void Run()
        {
            var sc = _context.Services.GetRequiredService<IServiceCollection>();
            _startup.ConfigureServices(sc);
            _context.Services = sc.BuildServiceProvider();

            var appBuilder = new ServiceAppBuilder(_context);
            _startup.ConfigurePipeline(appBuilder);
            _logger = _context.Services.GetService<ILogger<ServiceHost>>() ?? new NullLoggerFactory().CreateLogger<ServiceHost>();

            var delegates = appBuilder.Build();

            try
            {
                delegates(_context);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
            }
            
            Thread.Sleep(Timeout.Infinite);
        }

        private object[] GetMethodDependencies(ParameterInfo[] parameters)
        {
            var startupDep = new List<Object>();
            foreach (var parameterInfo in parameters)
            {
                startupDep.Add(_context.Services.GetRequiredService(parameterInfo.ParameterType));
            }

            return startupDep.ToArray();
        }
    }
}
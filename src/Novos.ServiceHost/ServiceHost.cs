using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Novos.ServiceHost
{
    public class ServiceHost
    {
        private readonly ServiceContext _context;
        private readonly IServiceStartup _startup;

        public ServiceHost(ServiceContext serviceProvider)
        {
            _context = serviceProvider;
            _startup = _context.Services.GetRequiredService<IServiceStartup>();
        }

        public void Run()
        {

            _startup.ConfigureServices(_context.Services.GetRequiredService<IServiceCollection>());

            var appBuilder = new ServiceAppBuilder(_context);
            _startup.ConfigurePipeline(appBuilder);

            var delegates = appBuilder.Build();

            delegates(_context);

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
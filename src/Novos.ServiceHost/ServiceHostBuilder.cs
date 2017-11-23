using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Novos.ServiceHost
{
    public class ServiceHostBuilder
    {
        private readonly IServiceCollection _serviceCollection = new ServiceCollection();
        private Type _startupType;

        public ServiceHostBuilder AddConfiguration(Action<IConfigurationBuilder> configurationBuilder)
        {

            var builder = new ConfigurationBuilder();
            configurationBuilder.Invoke(builder);
            var configRoot = builder.Build();
            _serviceCollection.AddSingleton(configRoot);
            _serviceCollection.AddSingleton<IConfiguration>(configRoot);
            return this;
        }

        public ServiceHostBuilder AddLogging(Action<ILoggingBuilder> loggingBuilder)
        {
            _serviceCollection.AddLogging(loggingBuilder);
            return this;
        }

        public ServiceHost Build()
        {
            //TODO: Use own internal service collection?
            _serviceCollection.AddSingleton(_serviceCollection);
            _serviceCollection.AddSingleton<IServiceAppBuilder , ServiceAppBuilder>();
            _serviceCollection.AddSingleton(typeof(IServiceStartup), _startupType);

            var context = new ServiceContext
            {
                Services = _serviceCollection.BuildServiceProvider()
            };

            return new ServiceHost(context);
        }

        public ServiceHostBuilder SetStartup<T>()
        {
            _startupType = typeof(T);
            return this;
        }
    }
}

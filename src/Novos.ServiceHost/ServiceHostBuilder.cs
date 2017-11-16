using System;
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
            _serviceCollection.AddSingleton(builder.Build());
            return this;
        }

        public ServiceHostBuilder AddLogging(Action<ILoggingBuilder> loggingBuilder)
        {
            _serviceCollection.AddLogging(loggingBuilder);
            return this;
        }

        public ServiceHost Build()
        {
            _serviceCollection.AddSingleton(_serviceCollection);
            var context = new ServiceContext
            {
                Services = _serviceCollection.BuildServiceProvider()
            };

            return new ServiceHost(context, _startupType);
        }

        public ServiceHostBuilder SetStartup<T>()
        {
            _startupType = typeof(T);
            return this;
        }
    }
}

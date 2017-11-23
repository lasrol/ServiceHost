using Microsoft.Extensions.DependencyInjection;

namespace Novos.ServiceHost
{
    public interface IServiceStartup
    {
        void ConfigureServices(IServiceCollection containerBuilder);
        void ConfigurePipeline(IServiceAppBuilder serviceBuilder);
    }
}
    
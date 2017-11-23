using System;

namespace Novos.ServiceHost
{
    public class ServiceContext : IServiceContext
    {
        public IServiceProvider Services { get; set; }   
    }
}

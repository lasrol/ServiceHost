using System;
using System.Collections.Generic;
using System.Text;

namespace Novos.ServiceHost
{
    public interface IServiceContext
    {
        IServiceProvider Services { get; set; }
    }
}

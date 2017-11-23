using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Novos.ServiceHost
{
    public interface IServiceAppBuilder
    {
        void Run(Func<IServiceContext, Task> inlineTask);
        void UseTask<T>();
        void UseTask(Type type);
        void Use(Func<TaskDelegate, TaskDelegate> task);
        TaskDelegate Build();
    }
}
    
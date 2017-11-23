using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Novos.ServiceHost
{
    public delegate Task TaskDelegate(IServiceContext context);

    public class ServiceAppBuilder : IServiceAppBuilder
    {
        private readonly IServiceContext _serviceContext;
        private TaskDelegate _delegates;


        public ServiceAppBuilder(IServiceContext serviceContext)
        {
            _serviceContext = serviceContext;
        }

        public void Run(Func<IServiceContext, Task> inlineTask)
        {
            Use((del) =>
            {
                return async delegate(IServiceContext ctx)
                {
                    await inlineTask.Invoke(ctx);
                    await del(ctx);
                };
            });
        }

        public void UseTask<T>()
        {
            UseTask(typeof(T));
        }

        public void UseTask(Type type)
        {
            Use((del) =>
            {
                var task = Activator.CreateInstance(type, del);
                return (TaskDelegate)Delegate.CreateDelegate(typeof(TaskDelegate), task, "Invoke", true);
            });
        }

        public void Use(Func<TaskDelegate, TaskDelegate> task)
        {
            _delegates = task.Invoke(_delegates);
        }

        public TaskDelegate Build()
        {
            return (TaskDelegate)_delegates;
        }
    }
}
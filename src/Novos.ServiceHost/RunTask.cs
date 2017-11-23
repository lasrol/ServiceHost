using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Novos.ServiceHost
{
    public class RunTask
    {
        private readonly TaskDelegate _next;

        public RunTask(TaskDelegate next)
        {
            _next = next;
        }

        public virtual Task Invoke(IServiceContext context)
        {
            var logger = context.Services.GetService<ILogger<RunTask>>();
            logger.LogInformation("Running a task");
            return _next(context);
        }
    }
}

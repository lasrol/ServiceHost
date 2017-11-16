﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace Novos.ServiceHost
{
    public class ServiceHost
    {
        private readonly ServiceContext _context;
        private readonly Type _startupClassType;
        private CancellationTokenSource _cancelationTokenSource;

        public ServiceHost(ServiceContext serviceProvider, Type startupClassType)
        {
            _context = serviceProvider;
            _startupClassType = startupClassType;
        }

        public void Run()
        {
            if (_startupClassType == null)
                throw new InvalidOperationException("Cannot start host without a startup class defined");

            var startup = Activator.CreateInstance(_startupClassType);
            var configureServicesMethod = _startupClassType.GetMethod("ConfigureServices");
            var parameters = GetMethodDependencies(configureServicesMethod.GetParameters());

            configureServicesMethod.Invoke(startup, parameters);
            _context.Services = _context.Services.GetService<IServiceCollection>().BuildServiceProvider();

            var runMethod = _startupClassType.GetMethod("Run");

            while (true)
            {
                _cancelationTokenSource = new CancellationTokenSource();
                new Task(() => 
                    runMethod.Invoke(startup, new[] { _context }), 
                    _cancelationTokenSource.Token, 
                    TaskCreationOptions.LongRunning)
                    .Start();
            }
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
using System;
using System.Collections.Generic;

namespace DIContainer
{
    public class DIServiceCollection : IDisposable
    {
        private List<ServiceDescriptor> _serviceDescriptors = new List<ServiceDescriptor>();

        private List<Container> _diContainers = new List<Container>();

        public void RegisterSingleton<TService>()
        {
            _serviceDescriptors.Add(new ServiceDescriptor(typeof(TService), ServiceLifetime.Singleton));
        }

        public void RegisterSingleton<TService>(TService implementation)
        {
            _serviceDescriptors.Add(new ServiceDescriptor(implementation, ServiceLifetime.Singleton));
        }

        public void RegisterSingleton<TService, TImplementation>() where TImplementation : TService
        {
            _serviceDescriptors.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton));
        }

        public void RegisterSingleton<TService, TImplementation>(TService implementation) where TImplementation : TService
        {
            _serviceDescriptors.Add(new ServiceDescriptor(implementation, typeof(TService), ServiceLifetime.Singleton));
        }

        public void RegisterTransient<TService>()
        {
            _serviceDescriptors.Add(new ServiceDescriptor(typeof(TService), ServiceLifetime.Transient));
        }

        public void RegisterTransient<TService>(TService implementation)
        {
            _serviceDescriptors.Add(new ServiceDescriptor(implementation, ServiceLifetime.Transient));
        }

        public void RegisterTransient<TService, TImplementation>() where TImplementation : TService
        {
            _serviceDescriptors.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient));
        }

        public Container GenerateContainer()
        {
            var diContainer = new Container(_serviceDescriptors);
            _diContainers.Add(diContainer);
            return diContainer;
        }

        public void Dispose()
        {
            foreach(var service in _serviceDescriptors)
            {
                if (service.Implementation is IDisposable)
                {
                    ((IDisposable)service.Implementation).Dispose();
                }
            }

            foreach(var container in _diContainers)
            {
                container.Dispose();
            }
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIContainer
{
    public class DIContainer : IGraph<ServiceDescriptor>, IDisposable
    {
        private List<ServiceDescriptor> _serviceDescriptors;
        private List<object> _transientImplementations = new List<object>();
        private List<object> _singletonImplementations = new List<object>();

        public DIContainer(List<ServiceDescriptor> serviceDescriptors)
        {
            _serviceDescriptors = serviceDescriptors;
        }

        public object GetService(Type serviceType, ServiceDescriptor dependencyType = null)
        {
            var descriptor = _serviceDescriptors.SingleOrDefault(x => x.ServiceType == serviceType);

            UnityEngine.Debug.Log($"{serviceType.Name}");

            if (descriptor == null)
                throw new NotImplementedException($"Service of type {serviceType.Name} is not registered");

            if (descriptor.Implementation != null)
            {
                if(!_singletonImplementations.Contains(descriptor.Implementation))
                    _singletonImplementations.Add(descriptor.Implementation);
                    
                return descriptor.Implementation;
            }

            var actualType = descriptor.ImplementationType ?? descriptor.ServiceType;

            if (actualType.IsAbstract || actualType.IsInterface)
            {
                throw new Exception("Cannot instantiate abstract classes or interfaces");
            }

            var constructorInfo = actualType.GetConstructors().FirstOrDefault();

            object implementation = null;

            if (constructorInfo != null)
            {
                var parameters = constructorInfo.GetParameters().Select(x =>
                {
                    var param = _serviceDescriptors.FirstOrDefault(y => y.ServiceType == x.ParameterType);
                    if (param != null)
                        CheckCircularDependency(param, descriptor.ServiceType);

                    return GetService(x.ParameterType, descriptor); 
                }).ToArray();

                implementation = Activator.CreateInstance(actualType, parameters);
            }

            if (descriptor.Lifetime == ServiceLifetime.Singleton)
            {
                descriptor.Implementation = implementation;
                _singletonImplementations.Add(descriptor.Implementation);
            }
            else if (descriptor.Lifetime == ServiceLifetime.Transient)
            {
                _transientImplementations.Add(implementation);
            }

            return implementation;
        }

        private void CheckCircularDependency(ServiceDescriptor parameter, Type original)
        {
            var result = this.DepthFirstTraversal(parameter);

            if (result.Where(x => x.ServiceType == original).Any())
            {
                StringBuilder stringBuilder = new StringBuilder();

                foreach(var type in result)
                {
                    stringBuilder.Append($" {type.ServiceType.Name}");
                }
                throw new Exception($"Circlylar Dependency Start {original.Name} from parameter {parameter.ImplementationType.Name} {result.Count()} {stringBuilder}");
            }
        }

        public T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }

        public IEnumerable<ServiceDescriptor> GetNeighbours(ServiceDescriptor s)
        {
            var constructorDependencyInfo = s.ImplementationType.GetConstructors().FirstOrDefault();

            if (constructorDependencyInfo != null)
            {
                UnityEngine.Debug.Log($"{s.ImplementationType}");

                var parameters = constructorDependencyInfo.GetParameters().Select(x => x.ParameterType);

                var list = new List<ServiceDescriptor>();
                
                foreach(var param in parameters)
                {
                    list.AddRange(_serviceDescriptors.Where(x => x.ServiceType == param));
                }

                return list;
            }

            return Enumerable.Empty<ServiceDescriptor>();

        }

        public void Dispose()
        {
            foreach (var service in _transientImplementations)
            {
                if (service is IDisposable)
                {
                    ((IDisposable)service).Dispose();
                }
            }

            foreach (var service in _singletonImplementations)
            {
                if (service is IDisposable)
                {
                    ((IDisposable)service).Dispose();
                }
            }
        }
    }
}



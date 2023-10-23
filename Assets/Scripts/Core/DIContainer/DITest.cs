using System;
using System.Threading.Tasks;
using UnityEngine;

namespace DIContainer
{
    public class DITest : MonoBehaviour
    {
    
        public void Start()
        {
            var serviceCollection = new DIServiceCollection();
    
            serviceCollection.RegisterSingleton<IImplementationFirst, SomeClass>();
            serviceCollection.RegisterSingleton<IImplemenatationThird, SomeThirdClass>();
    
            var container = serviceCollection.GenerateContainer();
    
            var firstImplementation = container.GetService<IImplementationFirst>();
    
            firstImplementation.PrintRandom();
        }
    }
    
    public class SomeClass : IImplementationFirst
    {
        private Guid guid;
        public SomeClass(IImplementationSecond implementationSecond)
        {
            guid = Guid.NewGuid();
            Debug.Log($"SomeClass");
        }
    
        public void PrintRandom()
        {
            Debug.Log($"#diTest {guid} IImplementationFirst {UnityEngine.Random.Range(1,100)}");
        }
    }
    
    public class SomeThirdClass : IImplemenatationThird
    {
        public SomeThirdClass(IImplementationFirst implementationFirst)
        {
        }
    }
    
    public interface IImplemenatationThird
    {
    }
    
    public interface IImplementationSecond
    {
        public void PrintRandomSecond();
    }
    
    public interface IImplementationFirst
    {
        void PrintRandom();
    }
}

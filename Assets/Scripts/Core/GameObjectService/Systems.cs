using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameObjectService
{
    // After Systems Start
    [DefaultExecutionOrder(1)]
    public class Systems : MonoBehaviour
    {
        public static Systems Instanse;
    
        private List<SystemInfo> systems = new List<SystemInfo>();
    
        private void Awake()
        {
            if (Systems.Instanse == null)
            {
                Systems.Instanse = this;
            }
        }
    
        private async void Start()
        {
            this.systems = this.systems.OrderBy(x => x.InitOrder).ToList();
    
            foreach(var system in this.systems)
            {
                await system.System.Init();
            }
        }
    
        public void AddSystem(BaseSystem system, int initOrder)
        {
            this.systems.Add(new SystemInfo { InitOrder = initOrder, System = system });
        }
    
        public T GetSystem<T>()
        {
            foreach(var s in this.systems)
            {
                if (s.System.TryGetComponent<T>(out T systemResult))
                {
                    return systemResult;
                }
            }
    
            throw new SystemNotFoundException(typeof(T).FullName);
        }
    }
    
    public struct SystemInfo
    {
        public int InitOrder { get; set; }
    
        public BaseSystem System { get; set; }
    }
}


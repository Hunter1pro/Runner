using System.Threading.Tasks;
using UnityEngine;

namespace GameObjectService
{
    public abstract class BaseSystem : MonoBehaviour
    {
        private Systems system;
        protected abstract int initOrder { get; }

        private void Start()
        {
            this.system = Systems.Instanse;
            this.system.AddSystem(this, this.initOrder);
        }

        public abstract Task Init();

        public T GetSystem<T>()
        {
            return this.system.GetSystem<T>();
        }
    }
}

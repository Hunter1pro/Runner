using System.Threading.Tasks;
using GameObjectService;

namespace Game
{
    public class SpawnSystem : BaseSystem
    {
        protected override int initOrder { get; }
    
        public override Task Init()
        {
        
            return Task.CompletedTask;
        }
    }
}

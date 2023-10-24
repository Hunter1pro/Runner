using System.Threading.Tasks;
using GameObjectService;

namespace Game
{
    public class GameEntry : BaseSystem
    {
        protected override int _initOrder { get; }
        public override Task Init()
        {
            return Task.CompletedTask;
        }
    }
}


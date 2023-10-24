using System.Threading.Tasks;
using Game.Views;
using GameObjectService;
using UnityEngine;

namespace Game
{
    public class GameEntry : BaseSystem
    {
        protected override int _initOrder { get; }

        [SerializeField] 
        private GameEntryView _gameEntryView;
        
        public override Task Init()
        {
            // Load Charcter
            // Attach Camera
            // Spawn to Start
            // Pathfinding to End
            // Move Thought Pathfinding
            // Trigger Finish
            // Trigger Obstacle
            // Jump
            
            return Task.CompletedTask;
        }
    }
}


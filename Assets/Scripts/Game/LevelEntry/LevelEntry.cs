using System.Threading.Tasks;
using Game.Level.Systems;
using Game.Views;
using GameObjectService;
using UnityEngine;

namespace Game
{
    public class LevelEntry : BaseSystem
    {
        protected override int _initOrder { get; }

        [SerializeField] 
        private LevelCastView _levelCastView;

        [SerializeField] 
        private MapCreatorView _mapCreatorView;
    
        public override Task Init()
        {
            LevelCast levelCast = new LevelCast(_levelCastView);
            MapCreator mapCreator = new MapCreator(_mapCreatorView);
        
        
            return Task.CompletedTask;
        }
    }
}


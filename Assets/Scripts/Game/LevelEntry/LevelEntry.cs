using System.Threading.Tasks;
using DIContainer;
using Game.Level.Systems;
using Game.Utils;
using Game.Views;
using GameObjectService;
using HexLib;
using UnityEngine;
using Logger = Game.Utils.Logger;

namespace Game
{
    public class LevelEntry : BaseSystem
    {
        protected override int _initOrder { get; } = -1;

        [SerializeField] 
        private LevelCastView _levelCastView;

        [SerializeField] 
        private MapCreatorView _mapCreatorView;

        private Container _diContainer;
    
        public override Task Init()
        {
            _diContainer = GenerateLevelServices();
            
            return Task.CompletedTask;
        }

        private Container GenerateLevelServices()
        {
            if (_diContainer != null)
                _diContainer.Dispose();
            
            DIServiceCollection diServiceCollection = new DIServiceCollection();
            diServiceCollection.RegisterSingleton<ILevelCast, LevelCast>();
            diServiceCollection.RegisterSingleton<IMapCreator, MapCreator>();
            diServiceCollection.RegisterSingleton<ICustomLogger, Logger>();
            diServiceCollection.RegisterSingleton<ISpawnSystem, SpawnSystem>();
            diServiceCollection.RegisterSingleton<ILevelObjectsContainer, LevelObjectsContainer>();
            diServiceCollection.RegisterSingleton(_levelCastView);
            diServiceCollection.RegisterSingleton(_mapCreatorView);

            return diServiceCollection.GenerateContainer();
        }
    }
}


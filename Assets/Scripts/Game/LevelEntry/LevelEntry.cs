using System.Linq;
using System.Threading.Tasks;
using DIContainer;
using Game.Level.Systems;
using Game.Level.Views;
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

        [SerializeField] 
        private GameLevelView _gameLevelView;

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

            Logger logger = new Logger();
            MapCreator mapCreator = new MapCreator(_mapCreatorView, logger);
            
            DIServiceCollection diServiceCollection = new DIServiceCollection();
            
            diServiceCollection.RegisterSingleton<IMapInfo, MapCreator>(mapCreator);
            diServiceCollection.RegisterSingleton<IMapCreator, MapCreator>(mapCreator);
            diServiceCollection.RegisterSingleton<ICustomLogger, Logger>(logger);
            diServiceCollection.RegisterSingleton<ILevelCast, LevelCast>();
            diServiceCollection.RegisterSingleton<ISpawnSystem, SpawnSystem>();
            diServiceCollection.RegisterSingleton<ILevelObjectsContainer, LevelObjectsContainer>();
            diServiceCollection.RegisterSingleton<HexGridSystem>();
            diServiceCollection.RegisterSingleton<IGameLevelSystem, GameLevelSystem>();
            diServiceCollection.RegisterSingleton(_levelCastView);
            diServiceCollection.RegisterSingleton(_gameLevelView);

            var container = diServiceCollection.GenerateContainer();

            var gameLevelSystem = container.GetService<IGameLevelSystem>();

            // Save/Load Level
            // LevelEditor is Simpler than trying to calc level by random weights
            // with obstacle and bonus weights
            gameLevelSystem.SpawnLevel(_gameLevelView.LevelDatas.First());

            return container;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var touch = _diContainer.GetService<ILevelCast>().Touch();
                if (touch.exist)
                {
                    var hex = _diContainer.GetService<HexGridSystem>().GetHex(touch.hit.point);
                    
                    Debug.Log($"Q {hex.Q}, R {hex.R} S {hex.S}");
                }
            }
        }
    }
}


using System.Threading.Tasks;
using DIContainerLib;
using Game.Boot;
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

        private RootEntry _rootEntry;
        
        [SerializeField] 
        private LevelCastView _levelCastView;

        private DIContainer diDiContainer;
    
        public override Task Init()
        {
            _rootEntry = GetSystem<RootEntry>();
            diDiContainer = GenerateLevelServices();
            
            return Task.CompletedTask;
        }

        private DIContainer GenerateLevelServices()
        {
            if (diDiContainer != null)
                diDiContainer.Dispose();

            var levelContainer = _rootEntry.ResolveLevelProvider();
            
            DIServiceCollection diServiceCollection = new DIServiceCollection();
            
            diServiceCollection.RegisterSingleton<IMapCreator, MapCreator>();
            diServiceCollection.RegisterSingleton<ICustomLogger, Logger>();
            diServiceCollection.RegisterSingleton<ILevelCast, LevelCast>();
            diServiceCollection.RegisterSingleton<ISpawnSystem, SpawnSystem>();
            diServiceCollection.RegisterSingleton<IDownloadBundle, DownloadBundle>();
            diServiceCollection.RegisterSingleton<ILevelObjectsContainer, LevelObjectsContainer>();
            diServiceCollection.RegisterSingleton<HexGridSystem>();
            diServiceCollection.RegisterSingleton<IGameLevelSystem, GameLevelSystem>();
            
            diServiceCollection.RegisterSingleton<ILevelProvider, LevelProvider>(levelContainer.levelProvider);
            diServiceCollection.RegisterSingleton<IMapInfo, LevelProvider>(levelContainer.levelProvider);
            diServiceCollection.RegisterSingleton(levelContainer.layout);

            diServiceCollection.RegisterSingleton(_levelCastView);

            var container = diServiceCollection.GenerateContainer();

            var gameLevelSystem = container.GetService<IGameLevelSystem>();

            // Save/Load Level
            // LevelEditor is Simpler than trying to calc level by random weights
            // with obstacle and bonus weights
            gameLevelSystem.SpawnLevel(levelContainer.material);

            return container;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var touch = diDiContainer.GetService<ILevelCast>().Touch();
                if (touch.exist)
                {
                    var hex = diDiContainer.GetService<HexGridSystem>().GetHex(touch.hit.point);
                    
                    Debug.Log($"Q {hex.Q}, R {hex.R} S {hex.S}");
                }
            }
        }
    }
}


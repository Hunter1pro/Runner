using System;
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
        
        [SerializeField] 
        private LevelCastView _levelCastView;

        private DIContainer _diContainer;
    
        public override Task Init()
        {
            return Task.CompletedTask;
        }

        public (DIContainer diContainer, IDownloadBundle downloadBundle, HexGridSystem hexGridSystem) GenerateLevelServices((LevelProvider levelProvider, Layout layout, Material material) levelContainer, Action obstacleTrigger)
        {
            if (_diContainer != null)
                _diContainer.Dispose();

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
            gameLevelSystem.SpawnLevel(levelContainer.material, obstacleTrigger);

            _diContainer = container;

            return (container, container.GetService<IDownloadBundle>(), container.GetService<HexGridSystem>());
        }
    }
}


using System;
using System.Linq;
using System.Threading.Tasks;
using DIContainer;
using Game.Level.Data;
using Game.Level.Systems;
using Game.Level.Views;
using Game.Utils;
using GameObjectService;
using HexLib;
using Unity.Mathematics;
using UnityEngine;
using Logger = Game.Utils.Logger;

namespace Game.Boot
{
    public class RootEntry : BaseSystem
    {
        protected override int _initOrder { get; } = -2;
        
        [SerializeField] 
        private GameLevelView _gameLevelView;

        private LevelContainerFile _levelContainerFile = new LevelContainerFile();
        
        public override Task Init()
        {
            // load or download files
            
            return Task.CompletedTask;
        }

        public (ILevelProvider levelProvider, Layout layout, Material material) ResolveLevelProvider()
        {
            var levelDataContainer = SaveSystem<LevelDataContainer>.Load(_levelContainerFile);

            if (levelDataContainer == null && levelDataContainer.LevelDatas.Count > 0)
                throw new Exception("LevelDataContainer File Not Loaded");
            
            DIServiceCollection diServiceCollection = new DIServiceCollection();
            
            var layout = new Layout(Layout.Flat, levelDataContainer.Size, new float3(levelDataContainer.Size, 0, levelDataContainer.Size * Mathf.Sqrt(3) / 2));

            diServiceCollection.RegisterSingleton<IMapCreator, MapCreator>();
            diServiceCollection.RegisterSingleton<ICustomLogger, Logger>();
            diServiceCollection.RegisterSingleton(layout);

            var container = diServiceCollection.GenerateContainer();

            var mapCreator = container.GetService<IMapCreator>();
            var mapGameObject = new GameObject("HexMap");
            
            var levelData = levelDataContainer.LevelDatas.First();

            LevelProvider levelProvider = new LevelProvider(levelData);

            return (levelProvider, layout, _gameLevelView.LevelData.Material);
        }
    }
}

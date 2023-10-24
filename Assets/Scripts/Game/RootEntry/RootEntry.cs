using System;
using System.Linq;
using System.Threading.Tasks;
using DIContainerLib;
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
        
        public override async Task Init()
        {
            var levelContainer = ResolveLevelProvider();

            GetSystem<LevelEntry>().GenerateLevelServices(levelContainer);
            await GetSystem<GameEntry>().GenerateGameServices((levelContainer));
        }

        private (LevelProvider levelProvider, Layout layout, Material material) ResolveLevelProvider()
        {
            var levelDataContainer = SaveSystem<LevelDataContainer>.Load(_levelContainerFile);

            if (levelDataContainer == null && levelDataContainer.LevelDatas.Count > 0)
                throw new Exception("LevelDataContainer File Not Loaded");
            
            var layout = new Layout(Layout.Flat, levelDataContainer.Size, new float3(levelDataContainer.Size, 0, levelDataContainer.Size * Mathf.Sqrt(3) / 2));

            var levelData = levelDataContainer.LevelDatas.First();

            LevelProvider levelProvider = new LevelProvider(levelData);

            return (levelProvider, layout, _gameLevelView.LevelData.Material);
        }
    }
}


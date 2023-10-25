using System;
using System.Linq;
using System.Threading.Tasks;
using DIContainerLib;
using Game.Bonus;
using Game.Level.Data;
using Game.Level.Systems;
using Game.Level.Views;
using Game.Systems;
using GameObjectService;
using HexLib;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Boot
{
    public class RootEntry : BaseSystem, IMoveFinish
    {
        protected override int _initOrder { get; } = -2;
        
        [SerializeField] 
        private GameLevelView _gameLevelView;

        private BonusEntry _bonusEntry;

        private LevelContainerFile _levelContainerFile = new LevelContainerFile();
        private LevelDataContainer _levelDataContainer;

        private DIContainer _levelContainer;
        private DIContainer _gameContainer;
        
        private int _currentLevel = 0;
        
        public override async Task Init()
        {
            _levelDataContainer = SaveSystem<LevelDataContainer>.Load(_levelContainerFile);

            _bonusEntry = GetSystem<BonusEntry>();

            await GenerateServices(_currentLevel);
        }

        private async Task GenerateServices(int currentLevel)
        {
            var rootLevelContainer = ResolveLevelProvider(_levelDataContainer, currentLevel);

            var levelContainer = GetSystem<LevelEntry>().GenerateLevelServices(rootLevelContainer, ObstacleTrigger, CoinTrigger, _bonusEntry.BonusTrigger);
            _levelContainer = levelContainer.diContainer;
            
            _gameContainer = await GetSystem<GameEntry>().GenerateGameServices(rootLevelContainer, levelContainer.downloadBundle, levelContainer.hexGridSystem, this);
            
            _bonusEntry.ResolveBonusServises(_gameContainer.GetService<IMoveComponent>());
        }

        private (LevelProvider levelProvider, Layout layout, Material material) ResolveLevelProvider(LevelDataContainer levelDataContainer, int level)
        {
            if (levelDataContainer == null && levelDataContainer.LevelDatas.Count > 0)
                throw new Exception("LevelDataContainer File Not Loaded");
            
            var layout = new Layout(Layout.Flat, levelDataContainer.Size, new float3(levelDataContainer.Size, 0, levelDataContainer.Size * Mathf.Sqrt(3) / 2));

            var levelData = levelDataContainer.LevelDatas.Skip(level).First();

            LevelProvider levelProvider = new LevelProvider(levelData);

            return (levelProvider, layout, _gameLevelView.LevelData.Material);
        }

        public async void ObstacleTrigger()
        {
            // Show UI Menu
            // Restart CurrentLevel
            
            _levelContainer.Dispose();
            _gameContainer.Dispose();

            await GenerateServices(_currentLevel);
        }
        
        private void CoinTrigger(GameObject coin)
        {
            // Show Score and Add it
            GameObject.Destroy(coin);
            Debug.Log($"CoinCatched");
        }

        public async void MoveFinished(float3 position)
        {
            _levelContainer.Dispose();
            _gameContainer.Dispose();
            
            if (_currentLevel < _levelDataContainer.LevelDatas.Count - 1)
            {
                // Show UI Menu and Score
                // Move Camera to Character with WinAnimation

                GenerateServices(_currentLevel++);
            }
            else
            {
                // Show UI Score, Game End and Restart
                Debug.Log("GameFinished");
            }
        }
    }
}


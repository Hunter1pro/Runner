using System;
using System.Linq;
using System.Threading.Tasks;
using DIContainerLib;
using Game.Bonus;
using Game.Level.Data;
using Game.Level.Systems;
using Game.Level.Views;
using Game.Systems;
using Game.UI;
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
        private UIEntry _uiEntry;

        private LevelContainerFile _levelContainerFile = new LevelContainerFile();
        private LevelDataContainer _levelDataContainer;

        private DIContainer _levelContainer;
        private DIContainer _gameContainer;
        
        private int _currentLevel;
        private int _score;
        
        public override async Task Init()
        {
            _levelDataContainer = SaveSystem<LevelDataContainer>.Load(_levelContainerFile);

            _bonusEntry = GetSystem<BonusEntry>();
            _uiEntry = GetSystem<UIEntry>();

            await GenerateServices(_currentLevel);
        }

        private async Task GenerateServices(int currentLevel)
        {
            _score = 0;
            _uiEntry.Dispose();
            _uiEntry.StartGame(_currentLevel);
            
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

        private void ObstacleTrigger()
        {
            _levelContainer.Dispose();
            _gameContainer.Dispose();
            
            _uiEntry.ShowPopup(PopupType.GameLost, () =>
            {
                _ = GenerateServices(_currentLevel);
            });
        }
        
        private void CoinTrigger(GameObject coin)
        {
            GameObject.Destroy(coin);

            _score += _gameLevelView.CoinScore;
            _uiEntry.ScoreUpdate(_score);
        }

        public void MoveFinished(float3 position)
        {
            _levelContainer.Dispose();
            _gameContainer.Dispose();
            
            if (_currentLevel < _levelDataContainer.LevelDatas.Count - 1)
            {
                _uiEntry.ShowPopup(PopupType.GameWin, () =>
                {
                    _ = GenerateServices(_currentLevel++);
                });
            }
            else
            {
                _uiEntry.ShowPopup(PopupType.GameEnd, () =>
                {
                    _currentLevel = 0; 
                    _ = GenerateServices(_currentLevel);
                });
            }
        }
    }
}


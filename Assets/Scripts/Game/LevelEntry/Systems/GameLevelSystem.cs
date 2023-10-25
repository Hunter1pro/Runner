using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Level.Data;
using Game.Level.Views;
using Game.Utils;
using HexLib;
using UnityEngine;

namespace Game.Level.Systems
{
    public class GameLevelSystem : IGameLevelSystem
    {
        private IMapCreator _mapCreator;
        private ISpawnSystem _spawnSystem;
        private IDownloadBundle _downloadBundle;
        private ILevelObjectsContainer _levelObjectsContainer;
        private ILevelProvider _levelProvider;
        private HexGridSystem _hexGridSystem;
        private ICustomLogger _logger;
        
        private Dictionary<string, Hex> _currentMap = new ();
        private string PLAYER_TAG = "Player";
        
        public GameLevelSystem(IMapCreator mapCreator, IDownloadBundle downloadBundle, HexGridSystem hexGridSystem, ILevelProvider levelProvider, ISpawnSystem spawnSystem, 
            ILevelObjectsContainer levelObjectsContainer, ICustomLogger logger)
        {
            _mapCreator = mapCreator;
            _spawnSystem = spawnSystem;
            _downloadBundle = downloadBundle;
            _levelObjectsContainer = levelObjectsContainer;
            _levelProvider = levelProvider;
            _hexGridSystem = hexGridSystem;
            _logger = logger;
        }

        public async Task SpawnLevel(Material material, Action obstacleTrigger, Action<GameObject> coinTrigger, Action<GameObject, BonusType> bonusTrigger)
        {
            // When we have load level system create map object here
            var mapGameObject = _mapCreator.SpawnMap(_levelProvider.HexMap, material);
            _levelObjectsContainer.AddLevelObject(mapGameObject);

            foreach (var obstacle in _levelProvider.LevelData.ObstaclesDatas)
            {
                var obstacleAsset = await _downloadBundle.DownloadAsset(obstacle.AssetAddress);
                var obstacleInstance = GameObject.Instantiate(obstacleAsset,
                    _hexGridSystem.HexToPosition(obstacle.Coordinate), Quaternion.identity);
                
                obstacleInstance.GetComponent<TriggerSubscribe>().Subscribe(PLAYER_TAG, collideObject =>
                {
                    obstacleTrigger?.Invoke();
                });
                
                _levelObjectsContainer.AddLevelObject(obstacleInstance);
            }

            foreach (var coin in _levelProvider.LevelData.CoinDatas)
            {
                var coinAsset = await _downloadBundle.DownloadAsset(coin.AssetAddress);
                var coinInstance = GameObject.Instantiate(coinAsset, _hexGridSystem.HexToPosition(coin.Coordinate),
                    Quaternion.identity);
                
                coinInstance.GetComponent<TriggerSubscribe>().Subscribe(PLAYER_TAG, collideObject =>
                {
                    coinTrigger?.Invoke(coinInstance);
                });
                
                _levelObjectsContainer.AddLevelObject(coinInstance);
            }
            
            foreach (var bonus in _levelProvider.LevelData.BonusDatas)
            {
                var bonusAsset = await _downloadBundle.DownloadAsset(bonus.AssetAddress);
                var bonusInstance = GameObject.Instantiate(bonusAsset, _hexGridSystem.HexToPosition(bonus.Coordinate),
                    Quaternion.identity);
                
                bonusInstance.GetComponent<TriggerSubscribe>().Subscribe(PLAYER_TAG, collideObject =>
                {
                    bonusTrigger?.Invoke(bonusInstance, bonus.BonusType);
                });
                
                _levelObjectsContainer.AddLevelObject(bonusInstance);
            }
        }
    }

    public interface IGameLevelSystem
    {
        Task SpawnLevel(Material material, Action obstacleTrigger, Action<GameObject> coinTrigger, Action<GameObject, BonusType> bonusTrigger);
    }
}


using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Utils;
using HexLib;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

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
        
        // List possible levels
        // SaveSystem
        // Spawn Level
        // Finish Level Trigger
        // Level Props with Trigger kill/damage
        // Level Bonus Touch

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

        public async Task SpawnLevel(Material material)
        {
            // When we have load level system create map object here
            var mapGameObject = _mapCreator.SpawnMap(_levelProvider.HexMap, material);
            _levelObjectsContainer.AddLevelObject(mapGameObject);

            foreach (var obstacle in _levelProvider.LevelData.ObstaclesDatas)
            {
                var obstacleAsset = await _downloadBundle.DownloadAsset(obstacle.AssetAddress);
                var obstacleInstance = GameObject.Instantiate(obstacleAsset,
                    _hexGridSystem.HexToPosition(Hex.StringToHex(obstacle.Coordinate)), Quaternion.identity);
                
                _levelObjectsContainer.AddLevelObject(obstacleInstance);
            }
        }
    }

    public interface IGameLevelSystem
    {
        Task SpawnLevel(Material material);
    }
}


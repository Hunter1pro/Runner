using System.Linq;
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
        private HexGridSystem _hexGridSystem;
        private ICustomLogger _logger;
        
        // List possible levels
        // SaveSystem
        // Spawn Level
        // Finish Level Trigger
        // Level Props with Trigger kill/damage
        // Level Bonus Touch

        public GameLevelSystem(IMapCreator mapCreator, HexGridSystem hexGridSystem, ISpawnSystem spawnSystem, ICustomLogger logger)
        {
            _mapCreator = mapCreator;
            _spawnSystem = spawnSystem;
            _hexGridSystem = hexGridSystem;
            _logger = logger;
        }

        public GameObject SpawnLevel(LevelData levelData)
        {
            var mapGameObject = _spawnSystem.SpawnEmpty("HexMap");

            GameObject level = _mapCreator.SpawnMap(mapGameObject, 0, levelData.Weight, 0, levelData.Height, levelData.Material);

            int height = levelData.NextObstacleDistance +
                         Random.Range(-levelData.RandomRangeDistance, levelData.RandomRangeDistance + 1);

            var rootHex = _hexGridSystem.GetHex(float3.zero);
            var hexList = _hexGridSystem.GetHexAround(float3.zero, height);

            var existHexList = hexList.Where(hex => _hexGridSystem.ExistInMap(hex) && rootHex.Distance(hex) >= height).ToList();
            
            if (existHexList.Count > 0)
            {
                var obstacle = _spawnSystem.Spawn(levelData.ObstacleAssets[Random.Range(0, levelData.ObstacleAssets.Count)], 
                    mapGameObject.transform);

                obstacle.transform.position = _hexGridSystem.HexToPosition(existHexList[Random.Range(0, existHexList.Count)]);
            }
            else
            {
                _logger.LogError($"{nameof(GameLevelSystem)} Hex not found for spawning obstacle");
            }
            
            return level;
        }
    }

    public interface IGameLevelSystem
    {
        GameObject SpawnLevel(LevelData levelData);
    }
}


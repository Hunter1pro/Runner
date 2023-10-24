using System.Collections.Generic;
using HexLib;
using UnityEngine;

namespace Game.Level.Data
{
    public class LevelDataContainer
    {
        public List<LevelData> LevelDatas { get; set; } = new List<LevelData>();

        public float Size { get; set; } = 1;
    }

    public class LevelData
    {
        public Vector3 StartCoordinate { get; set; }
        public string EndCoordinate { get; set; }

        public Dictionary<string, Hex> HexMap { get; set; } = new Dictionary<string, Hex>();

        public List<ObstaclesData> ObstaclesDatas { get; set; } = new List<ObstaclesData>();
        public List<BonusData> BonusDatas { get; set; } = new List<BonusData>();
    }
    
    public class ObstaclesData
    {
        public string AssetAddress { get; set; }
        public string Coordinate { get; set; }
    }
    
    public enum BonusType { Speed, Fly, Etc }
    
    public class BonusData
    {
        public string AssetAddress { get; set; }
        public string CoordinateId { get; set; }
        public BonusType BonusType { get; set; }
    }
}

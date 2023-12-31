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
        public string SkyBox { get; set; }
        public Hex StartCoordinate { get; set; }
        public Hex EndCoordinate { get; set; }

        public Dictionary<string, Hex> HexMap { get; set; } = new Dictionary<string, Hex>();

        public List<ObstaclesData> ObstaclesDatas { get; set; } = new List<ObstaclesData>();
        public List<CoinData> CoinDatas { get; set; } = new List<CoinData>();
        public List<BonusData> BonusDatas { get; set; } = new List<BonusData>();
    }

    public class CoinData
    {
        public string AssetAddress { get; set; }
        public Hex Coordinate { get; set; }
    }
    
    public class ObstaclesData
    {
        public string AssetAddress { get; set; }
        public Hex Coordinate { get; set; }
    }
    
    public enum BonusType { Speed, Etc }
    
    public class BonusData
    {
        public string AssetAddress { get; set; }
        public Hex Coordinate { get; set; }
        public BonusType BonusType { get; set; }
    }
}

using System.Collections.Generic;
using Game.Level.Data;
using HexLib;
using UnityEngine;

namespace Game.Level.Systems
{
    public class LevelProvider : ILevelProvider
    {
        public LevelData LevelData { get; }
        public Dictionary<string, Hex> HexMap => LevelData.HexMap;
        
        public LevelProvider(LevelData levelData)
        {
            LevelData = levelData;
        }
    }

    public interface ILevelProvider : IMapInfo
    {
        public LevelData LevelData { get; }
    }
}


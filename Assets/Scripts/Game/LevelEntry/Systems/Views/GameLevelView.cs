using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Level.Views
{
    [Serializable]
    public class GameLevelView 
    { 
        [field: SerializeField] 
        public List<LevelData> LevelDatas { get; private set; }
    }
}


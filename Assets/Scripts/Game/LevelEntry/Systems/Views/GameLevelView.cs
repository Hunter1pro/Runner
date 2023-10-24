using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Level.Views
{
    [Serializable]
    public class GameLevelView 
    { 
        [field: SerializeField] 
        public LevelTestData LevelData { get; private set; }
    }
}


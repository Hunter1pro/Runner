using System;
using UnityEngine;

namespace Game.Bonus.Views
{
    [Serializable]
    public class BonusEntryView
    {
        [field: SerializeField] 
        public float BonusSpeed { get; private set; } = 8;
        
        [field: SerializeField] 
        public float BonusSpeedTime { get; private set; } = 2;
    }
}


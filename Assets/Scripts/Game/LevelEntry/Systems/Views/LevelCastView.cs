using System;
using UnityEngine;

namespace Game.Views
{
    [Serializable]
    public class LevelCastView
    {
        [field: SerializeField]
        public Camera Camera { get; private set; }

        [field: SerializeField]
        public LayerMask LayerMask { get; private set; }

        [field: SerializeField]
        public float Distance { get; private set; } = 500;
    }
}



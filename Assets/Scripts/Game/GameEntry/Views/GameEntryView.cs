using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Views
{
    [Serializable]
    public class GameEntryView
    {
        [field: SerializeField] 
        public CinemachineVirtualCamera VirtualCamera;

        [field: SerializeField] 
        public AssetReference CharacterAsset;

        [field: SerializeField] 
        public float Speed { get; private set; } = 5;
        
        [field: SerializeField]
        public float SwipeScreenRatio { get; private set; } = 0.25f;
    }
}



using System;
using Cinemachine;
using UnityEngine;

namespace Game.Views
{
    [Serializable]
    public class GameEntryView
    {
        [field: SerializeField] 
        public CinemachineVirtualCamera VirtualCamera;

        [field: SerializeField] 
        private string CharacterAsset = "unitychan";
    }
}


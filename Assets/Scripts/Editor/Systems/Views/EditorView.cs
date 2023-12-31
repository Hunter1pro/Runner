using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.Editor.Systems.Views
{
    [Serializable]
    public class EditorView
    {
        [field: SerializeField] 
        public RectTransform Panel { get; private set; }
        
        [field: SerializeField] 
        public Transform LevelsPanelRoot { get; private set; }

        [field: SerializeField] 
        public float Size { get; private set; } = 1;
        
        [field: SerializeField] 
        public Transform ActionsPanelRoot { get; private set; }
        
        [field: SerializeField] 
        public TMP_InputField InputFieldPrefab { get; private set; }
        
        [field: SerializeField]
        public ButtonView ButtonViewPrefab { get; private set; }
        
        [field: SerializeField]
        public ButtonItem ButtonItemViewPrefab { get; private set; }
        
        [field: SerializeField]
        public GameObject StartPoint { get; private set; }
        
        [field: SerializeField]
        public GameObject EndPoint { get; private set; }

        [field: SerializeField] 
        public LevelTestData LevelTestData { get; private set; }

        [field: SerializeField] 
        public TMP_Dropdown DropdownPrefab { get; private set; }
        
        [field: SerializeField]
        public Color HiglightColor { get; private set; }
    }
}



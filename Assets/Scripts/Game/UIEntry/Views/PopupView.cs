using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PopupView
{
    [field: SerializeField]
    public TMP_Text HeaderText { get; private set; }
    
    [field: SerializeField]
    public Button Button { get; private set; } 
    
    [field: SerializeField]
    public TMP_Text ButtonText { get; private set; }
    
    [field: SerializeField]
    public GameObject Panel { get; private set; }
}

using System;
using TMPro;
using UnityEngine;

[Serializable]
public class TopPanelView
{
    [field: SerializeField]
    public TMP_Text LevelText { get; private set; }
    
    [field: SerializeField]
    public TMP_Text ScoreText { get; private set; }
}

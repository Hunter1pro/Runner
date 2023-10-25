using System.Collections.Generic;
using Game.Level.Views;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelTestData", menuName = "Level/LevelTestData")]
public class LevelTestData : ScriptableObject
{
    [field: SerializeField] 
    public Material Material { get; private set; }

    [field: SerializeField] 
    public int Height { get; private set; } = 100;
    
    [field: SerializeField] 
    public int Weight { get; private set; } = 3;
    
    [field: SerializeField] 
    public List<string> ObstacleAssets { get; private set; }
    
    [field: SerializeField]
    public string CoinAsset { get; private set; }
    
    // SkyBox
}
